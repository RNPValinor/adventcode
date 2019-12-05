const fs = require("fs");
const Intcode = require("../utils/Intcode");
const BaseDay = require("./BaseDay");

class Day2 extends BaseDay {
  constructor() {
    super();
    this.noun = 12;
    this.verb = 2;
    this.targetOutput = 19690720;
  }

  _getIntcode() {
    if (!this._intcode) {
      const data = fs.readFileSync("inputs/2.txt").toString();

      this._intcode = new Intcode(data);
    }

    return this._intcode;
  }

  runPart1() {
    const intcode = this._getIntcode();

    intcode.runProgram({
      inputs: [this.noun, this.verb],
      legacyInputMode: true
    });

    return intcode.currentData[0];
  }

  runPart2() {
    const intcode = this._getIntcode();

    for (let noun = 0; noun <= 99; noun++) {
      for (let verb = 0; verb <= 99; verb++) {
        intcode.runProgram({ inputs: [noun, verb], legacyInputMode: true });

        if (intcode.currentData[0] === this.targetOutput) {
          return `noun: ${noun}, verb: ${verb}`;
        }
      }
    }

    return "No noun/verb combo found!";
  }
}

module.exports = Day2;
