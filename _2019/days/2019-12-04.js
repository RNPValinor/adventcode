const BaseDay = require("./BaseDay");
const getDigits = require("../utils/getDigits");
const _ = require("lodash");

class Day4 extends BaseDay {
  constructor() {
    super();
    this.start = 172851;
    this.end = 675869;
  }

  _countValidPasswords(digitsCheck) {
    let start = this.start;
    const passwords = [];

    while (start <= this.end) {
      const digits = getDigits(start);
      let lastDigit = digits[0];

      // Check for decreasing digits - if we find one skip some numbers
      for (let i = 1, len = digits.length; i < len; i++) {
        const digit = digits[i];

        if (digit < lastDigit) {
          while (i < len) {
            digits[i] = lastDigit;
            i++;
          }
        } else {
          lastDigit = digit;
        }
      }

      start =
        digits[0] * 100000 +
        digits[1] * 10000 +
        digits[2] * 1000 +
        digits[3] * 100 +
        digits[4] * 10 +
        digits[5];

      if (start > this.end) {
        break;
      }

      if (digitsCheck(start)) {
        // console.log(`Found potential password: ${start}`);
        passwords.push(start);
      }

      start++;
    }

    return passwords.length;
  }

  runPart1() {
    return this._countValidPasswords(N => {
      return /([0-9]).*?\1/.test(N);
    });
  }

  runPart2() {
    return this._countValidPasswords(N => {
      return (
        /([0-9])((?!\1)[0-9])\2(?!\2)/.test(N) || /^([0-9])\1(?!\1)/.test(N)
      );
    });
  }
}

module.exports = Day4;
