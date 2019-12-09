const BaseDay = require("./BaseDay");
const fs = require("fs");
const Intcode = require("../utils/Intcode");
const IntcodeStates = require("../utils/IntcodeStates");
const permute = require("../utils/permute");

class Day7 extends BaseDay {
  _getAmplifier() {
    if (!this._data) {
      this._data = fs.readFileSync("inputs/7.txt").toString();
    }

    return new Intcode(this._data, this.verbose);
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

  runPart2() {
    const ampA = this._getAmplifier();
    const ampB = this._getAmplifier();
    const ampC = this._getAmplifier();
    const ampD = this._getAmplifier();
    const ampE = this._getAmplifier();

    let maxOutput = 0;

    for (var phases of permute([5, 6, 7, 8, 9])) {
      let terminated = false;

      ampA.runProgram({ inputs: [0, phases[0]] });
      ampB.runProgram({ inputs: [ampA.outputs.pop(), phases[1]] });
      ampC.runProgram({ inputs: [ampB.outputs.pop(), phases[2]] });
      ampD.runProgram({ inputs: [ampC.outputs.pop(), phases[3]] });
      ampE.runProgram({ inputs: [ampD.outputs.pop(), phases[4]] });

      do {
        ampA.inputs.push(ampE.outputs.pop());
        ampA.resumeExecution();
        ampB.inputs.push(ampA.outputs.pop());
        ampB.resumeExecution();
        ampC.inputs.push(ampB.outputs.pop());
        ampC.resumeExecution();
        ampD.inputs.push(ampC.outputs.pop());
        ampD.resumeExecution();
        ampE.inputs.push(ampD.outputs.pop());
        ampE.resumeExecution();

        terminated = ampE.state === IntcodeStates.Terminated;
      } while (!terminated);

      maxOutput = Math.max(maxOutput, ampE.outputs.pop());
    }

    return maxOutput;
  }
}

module.exports = Day7;
