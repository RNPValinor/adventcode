const BaseDay = require("./BaseDay");
const Intcode = require("../utils/Intcode");
const fs = require("fs");

class Day5 extends BaseDay {
  _getIntcode() {
    if (!this._intcode) {
      const data = fs.readFileSync("inputs/5.txt").toString();

      this._intcode = new Intcode(data);
    }

    return this._intcode;
  }

  runPart1() {
    const intcode = this._getIntcode();

    intcode.runProgram({ inputs: [1] });

    return intcode.outputs.pop();
  }

  runPart2() {
    const intcode = this._getIntcode();

    intcode.runProgram({ inputs: [5] });

    return intcode.outputs.pop();
  }
}

module.exports = Day5;
