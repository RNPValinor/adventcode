const BaseDay = require("./BaseDay");
const fs = require("fs");
const Intcode = require("../utils/Intcode");

class Day9 extends BaseDay {
  _getIntcode() {
    if (!this._intcode) {
      const data = fs.readFileSync("inputs/9.txt").toString();

      this._intcode = new Intcode(data, this.verbose);
    }

    return this._intcode;
  }

  runPart1() {
    const intcode = this._getIntcode();

    const exitStatus = intcode.runProgram({ inputs: [1] });

    if (this.verbose) {
      console.log(`Intcode exited with status ${exitStatus}`);
    }

    return intcode.outputs.join(",");
  }

  runPart2() {
    const intcode = this._getIntcode();

    const exitStatus = intcode.runProgram({ inputs: [2] });

    if (this.verbose) {
      console.log(`Intcode exited with status ${exitStatus}`);
    }

    return intcode.outputs.join(",");
  }
}

module.exports = Day9;
