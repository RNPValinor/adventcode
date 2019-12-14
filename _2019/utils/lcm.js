const _ = require("lodash");

function lcm(numbers) {
  function gcd(a, b) {
    return !b ? a : gcd(b, a % b);
  }

  function lcm(a, b) {
    return (a * b) / gcd(a, b);
  }

  var multiple = _.min(numbers);

  numbers.forEach(function(n) {
    multiple = lcm(multiple, n);
  });

  return multiple;
}

module.exports = lcm;
