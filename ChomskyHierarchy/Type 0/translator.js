/*
  Turing Machine to Type 0 grammar translator.
  
  Conventions: 
  1) Turing Machine should have only one final state;
  2) Turing Machine should NOT have states called 's', 's0' and 'f';
  3) Turing Machine should NOT have a rule that leave it idle;
*/

'use strict';

var fs  = require('fs');

var mt_file = 'mt.txt';
var grammar_file = 'grammar.txt';

var in_alphabet = ['1', '0', '_'], ext_alphabet = ['x'];
var out_alphabet = in_alphabet.concat(ext_alphabet), init_state = 'u0', final_state = 'halt';
var letters = [], buffer = new Buffer();

/* Some helpers */

Array.prototype.toString = function() {
  return '[' + this.join(',') + ']';
}

for (var i = 0; i < in_alphabet.length; i++) {
  for (var j = 0; j < out_alphabet.length; j++) {
    letters.push([in_alphabet[i], out_alphabet[j]].toString());
  }
}

function Buffer() {
  this.content = '';
}

Buffer.prototype.add_production = function (from, to) {
  this.content += "'" + from + "'" + ' -> ' + "'" + to + "'" + '\r\n';
}

Buffer.prototype.save = function (file) {
  fs.appendFileSync(file, this.content);
}

/* Algorithm */

buffer.add_production('s', '[_,_]s|s[_,_]|s0');
buffer.add_production('s0', 's0[0,0]|s0[1,1]');
buffer.add_production('s0', init_state);

fs.readFileSync(mt_file).toString().split('\r\n').forEach(function (line) {
  // skip comments and empty lines
  if (!line[0] || ['#', '\n', '\r\n', ' '].indexOf(line[0]) !== -1) return;

  var tokens = line.split(' ');
  var rule = {
    init_state: tokens[0],
    final_state: tokens[4],
    direction: tokens[3],
    cur_char: tokens[1],
    next_char: tokens[2]
  }
  
  if (rule.direction == 'l') {
    for (var i = 0; i < letters.length; i++) {
      for (var j = 0; j < in_alphabet.length; j++) {
        buffer.add_production(letters[i] + rule.init_state + [in_alphabet[j], rule.cur_char], 
          rule.final_state + letters[i] + [in_alphabet[j], rule.next_char]);
      }
    }
  } else {
    for (var j = 0; j < in_alphabet.length; j++) {
      buffer.add_production(rule.init_state + [in_alphabet[j], rule.cur_char],
        [in_alphabet[j], rule.next_char] + rule.final_state);
    }
  } 
});

buffer.add_production(final_state, 'f');

letters.forEach(function(letter) {
  var init_char = letter.substring(2, 1);
  var prod = init_char == '_' ? 'f' : 'f' + init_char + 'f';
  
  buffer.add_production('f' + letter, prod);
  buffer.add_production(letter + 'f', prod);
});

buffer.add_production('f', '');

buffer.save(grammar_file);