'use strict';

var fs  = require('fs');
var mt_file = 'mt.txt';
var grammar_file = 'grammar.txt';

var alphabet = ['1', '0', '_', 'x'], letters = [], buffer = '';

for (var i = 0; i < alphabet.length; i++) {
  if (alphabet[i] == 'x') continue;
  for (var j = 0; j < alphabet.length; j++) {
    letters.push('[' + alphabet[i] + ',' + alphabet[j] + ']');
  }
}

function write_production(from, to) {
  buffer += "'" + from + "'" + ' -> ' + "'" + to + "'" + '\r\n';
}

write_production('s', '[_,_]s|s[_,_]|q0');
write_production('q0', 'q0[0,0]|q0[1,1]');
write_production('q0', 'u0');

fs.readFileSync('mt.txt').toString().split('\r\n').forEach(function (line) {
  if (!line[0] || ['#', '\n', '\r\n', ' '].indexOf(line[0]) !== -1) return; // skip comments and empty lines

  var buffer = line.split(' ');
  var rule = {
    init_state: buffer[0],
    final_state: buffer[4],
    direction: buffer[3],
    cur_char: buffer[1],
    next_char: buffer[2]
  }
  
  if (rule.direction == 'l') {
    for (var i = 0; i < letters.length; i++) {
      for (var j = 0; j < alphabet.length; j++) {
        if (alphabet[j] == 'x') continue;
        write_production(letters[i] + rule.init_state + '[' + alphabet[j] + ',' + rule.cur_char + ']', 
          rule.final_state + letters[i] + '[' + alphabet[j] + ',' + rule.next_char + ']');
      }
    }
  } else {
    for (var j = 0; j < alphabet.length; j++) {
      if (alphabet[j] == 'x') continue;
      write_production(rule.init_state + '[' + alphabet[j] + ',' + rule.cur_char + ']',
        '[' + alphabet[j] + ',' + rule.next_char + ']' + rule.final_state);
    }
  } 
});

write_production('halt', 'f');

letters.forEach(function(letter) {
  var init_char = letter.substring(2, 1);
  var prod = init_char == '_' ? 'f' : 'f' + init_char + 'f';
  
  write_production('f' + letter, prod);
  write_production(letter + 'f', prod);
});

write_production('f', '');

fs.appendFileSync(grammar_file, buffer);