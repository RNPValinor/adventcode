const BaseDay = require("./BaseDay");
const fs = require("fs");
const Intcode = require("../utils/Intcode");
const IntcodeStates = require("../utils/IntcodeStates");
const _ = require("lodash");
const ConsoleColours = require("../utils/ConsoleColours");

class Day11 extends BaseDay {
  _getIntcode() {
    if (!this._intcode) {
      const data = fs.readFileSync("inputs/11.txt").toString();

      this._intcode = new Intcode(data, this.verbose);
    }

    return this._intcode;
  }

  _readFromPanel(x, y) {
    if (this._panels[x]) {
      return this._panels[x][y] || 0;
    } else {
      return 0;
    }
  }

  _writeToPanel(x, y, val) {
    if (!this._panels[x]) {
      this._panels[x] = {};
    }

    this._panels[x][y] = val;

    this._minX = Math.min(this._minX, x);
    this._maxX = Math.max(this._maxX, x);
    this._minY = Math.min(this._minY, y);
    this._maxY = Math.max(this._maxY, y);
  }

  _doPainting() {
    this._minX = Infinity;
    this._maxX = -Infinity;
    this._minY = Infinity;
    this._maxY = -Infinity;

    const intcode = this._getIntcode();

    const vel = {
      x: 0,
      y: 0,
      dir: {
        x: 0,
        y: -1
      }
    };

    intcode.runProgram();

    let terminated = false;

    while (!terminated) {
      intcode.pushInput(this._readFromPanel(vel.x, vel.y));

      const programState = intcode.resumeExecution();

      const turn = intcode.popOutput();
      const paint = intcode.popOutput();

      this._writeToPanel(vel.x, vel.y, paint);

      let newXDir, newYDir;

      if (turn == 0) {
        // Left
        newYDir = -vel.dir.x;
        newXDir = vel.dir.y;
      } else {
        // Right
        newYDir = vel.dir.x;
        newXDir = -vel.dir.y;
      }

      vel.dir.x = newXDir;
      vel.dir.y = newYDir;

      vel.x += vel.dir.x;
      vel.y += vel.dir.y;

      terminated = programState === IntcodeStates.Terminated;
    }
  }

  runPart1() {
    this._panels = {};

    this._doPainting();

    return _.sumBy(_.values(this._panels), _.size);
  }

  runPart2() {
    this._panels = { 0: { 0: 1 } };

    this._doPainting();

    let reg = "\n";

    for (let y = this._minY; y <= this._maxY; y++) {
      for (let x = this._minX; x <= this._maxX; x++) {
        const colCode = this._readFromPanel(x, y);

        if (colCode === 1) {
          reg += ConsoleColours.BgWhite + " ";
        } else {
          reg += ConsoleColours.BgBlack + " ";
        }
      }

      reg += ConsoleColours.Reset + "\n";
    }

    return reg;
  }
}

module.exports = Day11;
