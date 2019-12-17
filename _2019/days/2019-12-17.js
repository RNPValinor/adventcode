const BaseDay = require("./BaseDay");
const fs = require("fs");
const Intcode = require("../utils/Intcode");
const IntcodeStates = require("../utils/IntcodeStates");

class Day17 extends BaseDay {
  _getIntcode() {
    if (!this._intcode) {
      const data = fs.readFileSync("inputs/17.txt").toString();

      this._intcode = new Intcode(data, this.verbose);
    }

    return this._intcode;
  }

  _getScaffold() {
    const intcode = this._getIntcode();

    intcode.runProgram();

    const lines = intcode.outputs
      .map(v => String.fromCharCode(v))
      .join("")
      .split("\n");

    const scaffold = {};
    const potentialIntersections = [];
    let mapString = "";
    let robotPos;
    let numScaffoldPieces = 0;

    for (let y = 0, maxY = lines.length; y < maxY; y++) {
      const line = lines[y];

      for (let x = 0, maxX = line.length; x < maxX; x++) {
        const char = line[x];
        mapString += char;

        if (
          char === "#" ||
          char === "^" ||
          char === "v" ||
          char === ">" ||
          char === "<"
        ) {
          // Bit of scaffold
          if (!scaffold[x]) {
            scaffold[x] = {};
          }

          scaffold[x][y] = char;

          numScaffoldPieces++;

          if (scaffold[x][y - 1] || (scaffold[x - 1] && scaffold[x - 1][y])) {
            potentialIntersections.push({ x, y });
          }
        }

        if (char === "^" || char === "v" || char === ">" || char === "<") {
          robotPos = { x, y };
        }
      }

      mapString += "\n";
    }

    return {
      scaffold,
      mapString,
      potentialIntersections,
      robotPos,
      numScaffoldPieces
    };
  }

  runPart1() {
    const { scaffold, mapString, potentialIntersections } = this._getScaffold();

    let sumAlignment = 0;

    for (let i = 0, len = potentialIntersections.length; i < len; i++) {
      const { x, y } = potentialIntersections[i];

      let numAdjacent = 0;

      // Check up
      if (scaffold[x][y - 1]) {
        numAdjacent++;
      }

      // Check down
      if (scaffold[x][y + 1]) {
        numAdjacent++;
      }

      // Check left
      if (scaffold[x - 1] && scaffold[x - 1][y]) {
        numAdjacent++;
      }

      // Check right
      if (scaffold[x + 1] && scaffold[x + 1][y]) {
        numAdjacent++;
      }

      if (numAdjacent > 2) {
        sumAlignment += x * y;

        if (this.verbose) {
          console.log(`Found intersection at (${x}, ${y})`);
        }
      }
    }

    return `${sumAlignment}\n${mapString}`;
  }

  runPart2() {
    // const { scaffold, robotPos, numScaffoldPieces } = this._getScaffold();

    // const robotChar = scaffold[robotPos.x][robotPos.y];
    // const robotVel = {
    //   x: robotChar === "<" ? -1 : robotChar === ">" ? 1 : 0,
    //   y: robotChar === "^" ? -1 : robotChar === "v" ? 1 : 0
    // };

    // const instructions = [];

    // const visited = {};

    // visited[robotPos.x] = {};
    // visited[robotPos.x][robotPos.y] = true;

    // let numVisited = 1;
    // let numMovedSinceTurn = 0;

    // while (numVisited < numScaffoldPieces) {
    //   let nextX = robotPos.x + robotVel.x;
    //   let nextY = robotPos.y + robotVel.y;

    //   if (scaffold[nextX] && scaffold[nextX][nextY]) {
    //     // Is scaffold here; continue
    //     if (!visited[nextX] || !visited[nextX][nextY]) {
    //       numVisited++;

    //       if (!visited[nextX]) {
    //         visited[nextX] = {};
    //       }

    //       visited[nextX][nextY] = true;
    //     }
    //     numMovedSinceTurn++;
    //     robotPos.x = nextX;
    //     robotPos.y = nextY;
    //   } else {
    //     if (numMovedSinceTurn) {
    //       instructions.push(numMovedSinceTurn);
    //     }

    //     numMovedSinceTurn = 0;

    //     // No scaffold on the next step; turn
    //     if (robotVel.y === 0) {
    //       // Currently moving in the x direction; check whether we want to go up or down next
    //       if (scaffold[robotPos.x][robotPos.y + 1]) {
    //         // Is scaffold down; move there
    //         instructions.push(robotVel.x === 1 ? "R" : "L");
    //         robotVel.x = 0;
    //         robotVel.y = 1;
    //       } else {
    //         instructions.push(robotVel.x === 1 ? "L" : "R");
    //         robotVel.x = 0;
    //         robotVel.y = -1;
    //       }
    //     } else {
    //       // Currently moving in the y direction; check whether we want to go left or right next
    //       if (
    //         scaffold[robotPos.x + 1] &&
    //         scaffold[robotPos.x + 1][robotPos.y]
    //       ) {
    //         // Is scaffold right; move there
    //         instructions.push(robotVel.y === 1 ? "L" : "R");
    //         robotVel.x = 1;
    //         robotVel.y = 0;
    //       } else {
    //         instructions.push(robotVel.y === 1 ? "R" : "L");
    //         robotVel.x = -1;
    //         robotVel.y = 0;
    //       }
    //     }
    //   }
    // }

    // if (numMovedSinceTurn > 0) {
    //   instructions.push(numMovedSinceTurn);
    // }

    // return instructions.join(",");

    // Yay hardcoded!

    const intcode = this._getIntcode();

    const inputString =
      "A,A,B,C,A,C,A,B,C,B\nR,12,L,8,R,6\nR,12,L,6,R,6,R,8,R,6\nL,8,R,8,R,6,R,12\nn\n";
    let intPtr = 0;

    let state = intcode.runProgram({
      dataReplacement: data => (data[0] = 2)
    });

    while (state === IntcodeStates.Waiting) {
      intcode.pushInput(inputString.charCodeAt(intPtr++));
      state = intcode.resumeExecution();
    }

    return intcode.popOutput();
  }
}

module.exports = Day17;
