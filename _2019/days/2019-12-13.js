const BaseDay = require("./BaseDay");
const Intcode = require("../utils/Intcode");
const fs = require("fs");
const ConsoleColours = require("../utils/ConsoleColours");
const IntcodeStates = require("../utils/IntcodeStates");

class Day13 extends BaseDay {
  _getArcade() {
    if (!this._arcade) {
      const data = fs.readFileSync("inputs/13.txt").toString();

      this._arcade = new Intcode(data, this.verbose);
    }

    return this._arcade;
  }

  _getScreenState(arcade) {
    const screen = {};
    let maxX = 0;
    let maxY = 0;

    for (let i = 0, len = arcade.outputs.length; i < len - 2; i += 3) {
      const x = arcade.outputs[i];
      const y = arcade.outputs[i + 1];
      const tile = arcade.outputs[i + 2];

      if (!screen[x]) {
        screen[x] = {};
      }

      screen[x][y] = tile;

      // Only increase maxX/Y if this is a drawable tile (i.e. not empty)
      if (tile) {
        maxX = Math.max(maxX, x);
        maxY = Math.max(maxY, y);
      }
    }

    return { screen, maxX, maxY };
  }

  _getScreenString({ screen, maxX, maxY }) {
    let output = "";

    for (let y = 0; y <= maxY; y++) {
      for (let x = 0; x <= maxX; x++) {
        if (screen[x] && screen[x][y]) {
          switch (screen[x][y]) {
            case 0:
              output += " ";
              break;
            case 1:
              // Wall
              output += `${ConsoleColours.BgWhite} ${ConsoleColours.Reset}`;
              break;
            case 2:
              // Block
              output += `${ConsoleColours.BgGreen} ${ConsoleColours.Reset}`;
              break;
            case 3:
              // Paddle
              output += `${ConsoleColours.FgRed}_${ConsoleColours.Reset}`;
              break;
            case 4:
              // Ball
              output += `${ConsoleColours.FgBlue}o${ConsoleColours.Reset}`;
              break;
          }
        } else {
          output += " ";
        }
      }

      output += "\n";
    }

    return output;
  }

  runPart1() {
    const arcade = this._getArcade();

    arcade.runProgram();

    let numBlock = 0;

    for (let i = 2, len = arcade.outputs.length; i < len; i += 3) {
      const output = arcade.outputs[i];

      if (output == 2) {
        numBlock++;
      }
    }

    const screenData = this._getScreenState(arcade);

    const screenString = this._getScreenString(screenData);

    return `${numBlock}\n${screenString}`;
  }

  runPart2() {
    const arcade = this._getArcade();

    let state = arcade.runProgram({
      dataReplacement: currentData => {
        currentData[0] = 2;
      }
    });

    while (state === IntcodeStates.Waiting) {
      arcade.pushInput(0);
      state = arcade.resumeExecution();
    }

    const screenData = this._getScreenState(arcade);
    const screenString = this._getScreenString(screenData);

    return `\n${screenString}`;
  }
}

module.exports = Day13;
