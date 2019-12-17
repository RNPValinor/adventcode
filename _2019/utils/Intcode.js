const _ = require("lodash");
const getDigits = require("./getDigits");
const IntcodeStates = require("./IntcodeStates");

class Intcode {
  constructor(data, verbose = false) {
    this.originalData = _.map(data.split(","), num => parseInt(num));
    this.verbose = verbose;
    this.state = IntcodeStates.Terminated;
  }

  runProgram({
    inputs = [],
    legacyInputMode = false,
    dataReplacement = () => {}
  } = {}) {
    this.state = IntcodeStates.Running;
    this.currentData = this.originalData.slice();

    dataReplacement(this.currentData);

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
    this.relativeBase = 0;

    this.resumeExecution();

    return this.state;
  }

  pushInput(val) {
    this.inputs.push(val);
  }

  popOutput() {
    return this.outputs.pop();
  }

  pauseExecution() {
    this.state = IntcodeStates.Waiting;
  }

  resumeExecution() {
    this.state = IntcodeStates.Running;

    do {
      this._runNextInstruction();
    } while (this.state === IntcodeStates.Running);

    return this.state;
  }

  _runNextInstruction() {
    const instructionData = this._readFromAddress(this.instPtr);

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
      case 9:
        this._adjustRelativeBase(modes);
        break;
      case 99:
        this.state = IntcodeStates.Terminated;
        break;
      default:
        console.error(`Unknown opcode encountered: ${opCode}`);
        break;
    }
  }

  _getParamAddresses(numParams, modes) {
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
      return this._readFromAddress(idx);
    } else if (mode === 1) {
      // Immediate mode - the value at position idx is the one we want
      return idx;
    } else if (mode === 2) {
      return this.relativeBase + this._readFromAddress(idx);
    }
  }

  _readFromAddress(addr) {
    return this.currentData[addr] || 0;
  }

  _doAdd(modes) {
    const [aAddr, bAddr, resAddr] = this._getParamAddresses(3, modes);

    const a = this._readFromAddress(aAddr);
    const b = this._readFromAddress(bAddr);

    if (this.verbose) {
      console.log(
        `Doing add, a: ${a}, b: ${b}, resAddr: ${resAddr} (set to ${a + b})`
      );
    }

    this.currentData[resAddr] = a + b;

    this.instPtr += 4;
  }

  _doMult(modes) {
    const [aAddr, bAddr, resAddr] = this._getParamAddresses(3, modes);

    const a = this._readFromAddress(aAddr);
    const b = this._readFromAddress(bAddr);

    if (this.verbose) {
      console.log(
        `Doing mult, a: ${a}, b: ${b}, resAddr: ${resAddr} (set to ${a * b})`
      );
    }

    this.currentData[resAddr] = a * b;

    this.instPtr += 4;
  }

  _readInput(modes) {
    if (this.inputs.length === 0) {
      this.pauseExecution();
      return;
    }

    const [ptr] = this._getParamAddresses(1, modes);

    const input = this.inputs.pop();

    if (this.verbose) {
      console.log(`Reading input: ${input}, storing in: ${ptr}`);
    }

    this.currentData[ptr] = input;

    this.instPtr += 2;
  }

  _writeOutput(modes) {
    const [ptr] = this._getParamAddresses(1, modes);

    const output = this._readFromAddress(ptr);

    if (this.verbose) {
      console.log(`Writing output: ${output}, read from: ${ptr}`);
    }

    this.outputs.push(output);

    this.instPtr += 2;
  }

  _jumpIfTrue(modes) {
    const [jmpAddr, newPtrAddr] = this._getParamAddresses(2, modes);

    const jmp = this._readFromAddress(jmpAddr);
    const newPtr = this._readFromAddress(newPtrAddr);

    if (this.verbose) {
      console.log(`Jump if true: ${jmp}, jump to ${newPtr}`);
    }

    if (jmp !== 0) {
      this.instPtr = newPtr;
    } else {
      this.instPtr += 3;
    }
  }

  _jumpIfFalse(modes) {
    const [jmpAddr, newPtrAddr] = this._getParamAddresses(2, modes);

    const jmp = this._readFromAddress(jmpAddr);
    const newPtr = this._readFromAddress(newPtrAddr);

    if (this.verbose) {
      console.log(`Jump if false: ${jmp}, jump to ${newPtr}`);
    }

    if (jmp === 0) {
      this.instPtr = newPtr;
    } else {
      this.instPtr += 3;
    }
  }

  _lessThan(modes) {
    const [aAddr, bAddr, resAddr] = this._getParamAddresses(3, modes);

    const a = this._readFromAddress(aAddr);
    const b = this._readFromAddress(bAddr);

    if (this.verbose) {
      console.log(`Check if ${a} < ${b}, store in ${resAddr} (got ${a < b})`);
    }

    this.currentData[resAddr] = a < b ? 1 : 0;

    this.instPtr += 4;
  }

  _equals(modes) {
    const [aAddr, bAddr, resAddr] = this._getParamAddresses(3, modes);

    const a = this._readFromAddress(aAddr);
    const b = this._readFromAddress(bAddr);

    if (this.verbose) {
      console.log(
        `Check if ${a} === ${b}, store in ${resAddr} (got ${a === b})`
      );
    }

    this.currentData[resAddr] = a === b ? 1 : 0;

    this.instPtr += 4;
  }

  _adjustRelativeBase(modes) {
    const [adjustmentAddr] = this._getParamAddresses(1, modes);

    const adjustment = this._readFromAddress(adjustmentAddr);

    if (this.verbose) {
      console.log(
        `Adjusting relative base by ${adjustment}, will become ${this
          .relativeBase + adjustment}`
      );
    }

    this.relativeBase += adjustment;

    this.instPtr += 2;
  }
}

module.exports = Intcode;
