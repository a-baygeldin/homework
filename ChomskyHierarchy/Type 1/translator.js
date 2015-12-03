/*
  LBA to Type 1 grammar translator. 
    
  Conventions: 
  1) LBA should have only one final state;
  2) LBA should NOT have states called 's' and 's0';
  3) LBA should NOT have a rule that leave it idle;
  4) LBA input alphabet includes two special symbols, serving as left and right endmarkers.
  5) LBA transitions may not print other symbols over the endmarkers.
  6) LBA transitions may neither move to the left of the left endmarker nor to the right of the right endmarker.
*/

'use strict';

var fs  = require('fs');

var lba_file = 'lba.txt';
var grammar_file = 'grammar.txt';

var in_alphabet = ['1', '0', '_'], ext_alphabet = ['x'], l_edge = '^', r_edge = '$';
var out_alphabet = in_alphabet.concat(ext_alphabet), init_state = 'u0', final_state = 'halt';
var buffer_1char = new Buffer(), buffer_nchar = new Buffer();

/* Some helpers */

Array.prototype.toString = function() {
  return '[' + this.join(',') + ']';
};

Object.defineProperty(Array.prototype, "distinct", {
  value: function() {
    return this.filter(function (value, index, self) {
      return self.indexOf(value) === index;
    });
  },
  enumerable: false
});

Object.defineProperty(Object.prototype, "isObject", {
  value: function(obj) {
    return typeof obj === 'object' && obj !== null;
  },
  enumerable: false
});

function Buffer() {
  this.content = '';
}

Buffer.prototype.add_production = function (init) {
  var buffer, productions = [init];
  var letters = Letter.getInstances(init.from.concat(init.to)).distinct();

  letters.forEach(function (letter) {
    buffer = [];
    productions.forEach(function (prod) {
      buffer.push.apply(buffer, letter.insertInto(prod));
    });
    productions = buffer;
  });

  for (var i = 0; i < productions.length; i++) {
    this.content += "'" + productions[i].from.join('') + "'" + ' -> '
      + "'" + productions[i].to.join('') + "'" + '\r\n';
  }
};

Buffer.prototype.save = function (file) {
  fs.appendFileSync(file, this.content);
};

function Letter(alphabet) {
  this.alphabet = alphabet;
}

Letter.getInstances = function (obj) {
  var letters = [];

  for (var key in obj) {
    if (Letter.isLetter(obj[key])) {
      letters.push(obj[key]);
    } else if (Object.isObject(obj[key])) {
      letters.push.apply(letters, Letter.getInstances(obj[key]));
    }
  }
  
  return letters;
};

Letter.isLetter = function (obj) {
  return obj instanceof Letter;
};

Letter.prototype.insertInto = function (obj) {
  var self = this, list = [];
  var isObject = Object.isObject(obj), isLetter = Letter.isLetter(obj);

  this.alphabet.forEach(function (char, index) {
    list[index] = isLetter ? (obj === self ? char : obj) : (Array.isArray(obj) ? [] : (isObject ? {} : obj));
  });

  if (isObject && !isLetter) {
    for (var key in obj) {
      this.insertInto(obj[key]).forEach(function (value, index) {
          list[index][key] = value;
      });
    }
  }

  return list;
};

/* Algorithm */

var z = new Letter(out_alphabet), a = new Letter(in_alphabet), b = new Letter(in_alphabet);

// all possible 1-char input:
buffer_1char.add_production({ from: ['s'], to: [[init_state, l_edge, a, a, r_edge]] });

// all possible n-char input:
buffer_nchar.add_production({ from: ['s'], to: [[init_state, l_edge, a, a], 's0'] });
buffer_nchar.add_production({ from: ['s0'], to: [[a, a], 's0'] });
buffer_nchar.add_production({ from: ['s0'], to: [[a, a, r_edge]] }); 

fs.readFileSync(lba_file).toString().split('\r\n').forEach(function (line) {
  // skip comments and empty lines
  if (!line[0] || ['#', '\n', '\r\n', ' '].indexOf(line[0]) !== -1) return;

  var tokens = line.split(' ');
  var rule = {
    init_state: tokens[0],
    final_state: tokens[4],
    direction: tokens[3],
    cur_char: tokens[1],
    next_char: tokens[2]
  };

  switch (rule.cur_char) {
    case l_edge: // (rule.init_state l_edge l_edge r rule.final_state)
      // simulate the LBA for 1-char input:
      buffer_1char.add_production({ from: [[rule.init_state, l_edge, z, a, r_edge]],
        to: [[l_edge, rule.final_state, z, a, r_edge]]});
      // simulate the LBA on the left side of the tape:
      buffer_nchar.add_production({ from: [[rule.init_state, l_edge, z, a]], 
        to: [[l_edge, rule.final_state, z, a]] });
      break;
    case r_edge: //(rule.init_state r_edge r_edge l rule.final_state)
      // simulate the LBA for 1-char input:
      buffer_1char.add_production({ from: [[l_edge, z, a, rule.init_state, r_edge]],
        to: [[l_edge, rule.final_state, z, a, r_edge]]});
      // simulate the LBA on the right side of the tape:
      buffer_nchar.add_production({ from: [[z, a, rule.init_state, r_edge]], 
        to: [[rule.final_state, z, a, r_edge]] });
      break;
    default:
      switch (rule.direction) {
        case 'l': // (rule.init_state rule.cur_char rule.next_char l rule.final_state)
          // simulate the LBA for 1-char input:
          buffer_1char.add_production({ from: [[l_edge, rule.init_state, rule.cur_char, a, r_edge]], 
            to: [[rule.final_state, l_edge, rule.next_char, a, r_edge]] });
          // simulate the LBA on the left side of the tape:
          buffer_nchar.add_production({ from: [[l_edge, rule.init_state, rule.cur_char, a]], 
            to: [[rule.final_state, l_edge, rule.next_char, a]] });
          // simulate the LBA in the middle of the tape:
          buffer_nchar.add_production({ from: [[z, b], [rule.init_state, rule.cur_char, a]], 
            to: [[rule.final_state, z, b], [rule.next_char, a]] });
          // simulate the LBA in the middle of the tape:
          buffer_nchar.add_production({ from: [[l_edge, z, b], [rule.init_state, rule.cur_char, a]], 
            to: [[l_edge, rule.final_state, z, b], [rule.next_char, a]] });
          // simulate the LBA on the right side of the tape:
          buffer_nchar.add_production({ from: [[z, b], [rule.init_state, rule.cur_char, a, r_edge]], 
            to: [[rule.final_state, z, b, rule.next_char, a, r_edge]] });
          // simulate the LBA on the right side of the tape:
          buffer_nchar.add_production({ from: [[l_edge, z, b], [rule.init_state, rule.cur_char, a, r_edge]], 
            to: [[l_edge, rule.final_state, z, b, rule.next_char, a, r_edge]] });
          break;
        case 'r': // (rule.init_state rule.cur_char rule.next_char r rule.final_state)
          // simulate the LBA for 1-char input:
          buffer_1char.add_production({ from: [[l_edge, rule.init_state, rule.cur_char, a, r_edge]], 
            to: [[l_edge, rule.next_char, a, rule.final_state, r_edge]] });
          // simulate the LBA on the left side of the tape:
          buffer_nchar.add_production({ from: [[l_edge, rule.init_state, rule.cur_char, a], [z, b]], 
            to: [[l_edge, rule.next_char, a], [rule.final_state, z, b]] });
          // simulate the LBA on the left side of the tape:
          buffer_nchar.add_production({ from: [[l_edge, rule.init_state, rule.cur_char, a], [z, b, r_edge]], 
            to: [[l_edge, rule.next_char, a], [rule.final_state, z, b, r_edge]] });
          // simulate the LBA in the middle of the tape:
          buffer_nchar.add_production({ from: [[rule.init_state, rule.cur_char, a], [z, b]], 
            to: [[rule.next_char, a], [rule.final_state, z, b]] });
          // simulate the LBA in the middle of the tape:
          buffer_nchar.add_production({ from: [[rule.init_state, rule.cur_char, a], [z, b, r_edge]], 
            to: [[rule.next_char, a], [rule.final_state, z, b, r_edge]] });
          // simulate the LBA on the right side of the tape:
          buffer_nchar.add_production({ from: [[rule.init_state, rule.cur_char, a, r_edge]], 
            to: [[rule.next_char, a, rule.final_state, r_edge]] });
          break;
      }
  }
  
});

// clears non-terminals for 1-char input:
buffer_1char.add_production({ from: [[final_state, l_edge, z, a, r_edge]], to: [a] });
buffer_1char.add_production({ from: [[l_edge, final_state, z, a, r_edge]], to: [a] });
buffer_1char.add_production({ from: [l_edge, z, a, final_state, r_edge], to: [a] });

// clears non-terminals on the left side of the tape:
buffer_nchar.add_production({ from: [[final_state, l_edge, z, a]], to: [a] });
buffer_nchar.add_production({ from: [[l_edge, z, final_state, a]], to: [a] });

// clears non-terminals in the middle of the tape:
buffer_nchar.add_production({ from: [[final_state, z, a]], to: [a] });

// clears non-terminals on the right side of the tape:
buffer_nchar.add_production({ from: [[final_state, z, a, r_edge]], to: [a] });
buffer_nchar.add_production({ from: [[z, a, final_state, r_edge]], to: [a] });

// clears non-terminals from left to right:
buffer_nchar.add_production({ from: [a, [z, b]], to: [a, b] });
buffer_nchar.add_production({ from: [a, [z, b, r_edge]], to: [a, b] });

// clears non-terminals from right to left:
buffer_nchar.add_production({ from: [[z, a], b], to: [a, b] });
buffer_nchar.add_production({ from: [[l_edge, z, a], b], to: [a, b] });

buffer_1char.save(grammar_file); buffer_nchar.save(grammar_file);
