const _ = require("lodash");
const getDigits = require("./getDigits");

const verbose = false;

class Intcode {
  constructor(data) {
    this.originalData = _.map(data.split(","), num => parseInt(num));
  }

  runProgram({ inputs, legacyInputMode = false }) {
    this.currentData = this.originalData.slice();

    if (legacyInputMode) {
      for (let i = 0, len = inputs.length; i < len; i++) {
        const input = inputs[i];
        this.currentData[i + 1] = input;
      }
    } else {
      this.inputs = inputs;
    }

    this.outputs = [];

    this.instPtr = 0;

    let exit = false;

    do {
      exit = this._runNextInstruction();
    } while (!exit);
  }

  _runNextInstruction() {
    const instructionData = this.currentData[this.instPtr];

    const opCode = instructionData % 100;
    const modes = (instructionData - opCode) / 100;

    switch (opCode) {
      case 1:
        this._doAdd(modes);
        break;
      case 2:
        this._doMult(modes);
        break;
      case 3:
        this._readInput(modes);
        break;
      case 4:
        this._writeOutput(modes);
        break;
      case 5:
        this._jumpIfTrue(modes);
        break;
      case 6:
        this._jumpIfFalse(modes);
        break;
      case 7:
        this._lessThan(modes);
        break;
      case 8:
        this._equals(modes);
        break;
      case 99:
      default:
        return true;
    }

    return false;
  }

  _readParameters(numParams, modes) {
    const params = [];
    const modeArray = getDigits(modes).reverse();

    for (let i = 0; i < numParams; i++) {
      params.push(this._loadValue(this.instPtr + i + 1, modeArray[i] || 0));
    }

    return params;
  }

  _loadValue(idx, mode) {
    if (mode === 0) {
      // Position mode - the value at position idx is a pointer
      return this.currentData[this.currentData[idx]];
    } else if (mode === 1) {
      // Immediate mode - the value at position idx is the one we want
      return this.currentData[idx];
    }
  }

  _doAdd(modes) {
    const [a, b] = this._readParameters(2, modes);
    const resAddr = this.currentData[this.instPtr + 3];

    if (verbose) {
      console.log(`Doing add, a: ${a}, b: ${b}, resAddr: ${resAddr}`);
    }

    this.currentData[resAddr] = a + b;

    this.instPtr += 4;
  }

  _doMult(modes) {
    const [a, b] = this._readParameters(2, modes);
    const resAddr = this.currentData[this.instPtr + 3];

    if (verbose) {
      console.log(`Doing mult, a: ${a}, b: ${b}, resAddr: ${resAddr}`);
    }

    this.currentData[resAddr] = a * b;

    this.instPtr += 4;
  }

  _readInput(modes) {
    const ptr = this.currentData[this.instPtr + 1];
    const input = this.inputs.pop();

    if (verbose) {
      console.log(`Reading input: ${input}, storing in: ${ptr}`);
    }

    this.currentData[ptr] = input;

    this.instPtr += 2;
  }

  _writeOutput(modes) {
    const ptr = this.currentData[this.instPtr + 1];
    const output = this.currentData[ptr];

    if (verbose) {
      console.log(`Writing output: ${output}, read from: ${ptr}`);
    }

    this.outputs.push(this.currentData[ptr]);

    this.instPtr += 2;
  }

  _jumpIfTrue(modes) {
    const [jmp, newPtr] = this._readParameters(2, modes);

    if (verbose) {
      console.log(`Jump if true: ${jmp}, jump to ${newPtr}`);
    }

    if (jmp !== 0) {
      this.instPtr = newPtr;
    } else {
      this.instPtr += 3;
    }
  }

  _jumpIfFalse(modes) {
    const [jmp, newPtr] = this._readParameters(2, modes);

    if (verbose) {
      console.log(`Jump if false: ${jmp}, jump to ${newPtr}`);
    }

    if (jmp === 0) {
      this.instPtr = newPtr;
    } else {
      this.instPtr += 3;
    }
  }

  _lessThan(modes) {
    const [a, b] = this._readParameters(2, modes);
    const resAddr = this.currentData[this.instPtr + 3];

    if (verbose) {
      console.log(`Check if ${a} < ${b}, store in ${resAddr}`);
    }

    this.currentData[resAddr] = a < b ? 1 : 0;

    this.instPtr += 4;
  }

  _equals(modes) {
    const [a, b] = this._readParameters(2, modes);
    const resAddr = this.currentData[this.instPtr + 3];

    if (verbose) {
      console.log(`Check if ${a} === ${b}, store in ${resAddr}`);
    }

    this.currentData[resAddr] = a === b ? 1 : 0;

    this.instPtr += 4;
  }
}

module.exports = Intcode;
