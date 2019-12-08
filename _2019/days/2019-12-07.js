const BaseDay = require("./BaseDay");
const fs = require("fs");
const Intcode = require("../utils/Intcode");
const permute = require("../utils/permute");

class Day7 extends BaseDay {
  _getAmplifier() {
    if (!this._amp) {
      const data = fs.readFileSync("inputs/7.txt").toString();

      this._amp = new Intcode(data, this.verbose);
    }

    return this._amp;
  }

  runPart1() {
    const amp = this._getAmplifier();

    let maxOutput = 0;

    for (var phases of permute([0, 1, 2, 3, 4])) {
      let curVal = 0;

      for (let i = 0, len = phases.length; i < len; i++) {
        const phase = phases[i];

        amp.runProgram({ inputs: [curVal, phase] });

        curVal = amp.outputs.pop();
      }

      if (curVal > maxOutput) {
        maxOutput = curVal;
      }
    }

    return maxOutput;
  }
}

module.exports = Day7;
