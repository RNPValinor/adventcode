const _ = require("lodash");

class Intcode {
  constructor(data) {
    this.originalData = _.map(data.split(","), num => parseInt(num));
  }

  runProgram(input1, input2) {
    this.currentData = this.originalData.slice();
    this.currentData[1] = input1;
    this.currentData[2] = input2;

    this.instPtr = 0;

    let exit = false;

    do {
      exit = this._runNextInstruction();
    } while (!exit);
  }

  _runNextInstruction() {
    const opCode = this.currentData[this.instPtr];

    switch (opCode) {
      case 1:
        this._doAdd();
        break;
      case 2:
        this._doMult();
        break;
      case 99:
      default:
        return true;
    }

    return false;
  }

  _doAdd() {
    const aPtr = this.currentData[this.instPtr + 1];
    const bPtr = this.currentData[this.instPtr + 2];
    const resPtr = this.currentData[this.instPtr + 3];

    this.currentData[resPtr] = this.currentData[aPtr] + this.currentData[bPtr];

    this.instPtr += 4;
  }

  _doMult() {
    const aPtr = this.currentData[this.instPtr + 1];
    const bPtr = this.currentData[this.instPtr + 2];
    const resPtr = this.currentData[this.instPtr + 3];

    this.currentData[resPtr] = this.currentData[aPtr] * this.currentData[bPtr];

    this.instPtr += 4;
  }
}

module.exports = Intcode;
