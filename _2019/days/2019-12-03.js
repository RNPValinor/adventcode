const fs = require("fs");
const BaseDay = require("./BaseDay");

class Day3 extends BaseDay {
  constructor(verbose) {
    super(verbose);
    this.paths = [];
    this.intersections = [];
  }

  _loadPaths() {
    if (this._pathsLoaded) {
      return;
    }

    const pathData = fs
      .readFileSync("inputs/3.txt")
      .toString()
      .split("\n");

    for (let i = 0, len = pathData.length; i < len; i++) {
      this._readPath(pathData[i]);
    }

    this._pathsLoaded = true;
  }

  _readPath(pathData) {
    const instructions = pathData.split(",");

    let curPos = {
      x: 0,
      y: 0
    };

    const path = {};
    let numSteps = 0;

    for (let i = 0, len = instructions.length; i < len; i++) {
      const instr = instructions[i];

      const dir = instr[0];
      let speed = parseInt(instr.substring(1));

      let dx = 0;
      let dy = 0;

      switch (dir) {
        case "U":
          dy = 1;
          break;
        case "D":
          dy = -1;
          break;
        case "L":
          dx = -1;
          break;
        case "R":
          dx = 1;
          break;
      }

      while (speed > 0) {
        speed--;
        numSteps++;

        curPos.x += dx;
        curPos.y += dy;

        this._setVisited(curPos.x, curPos.y, path, numSteps);
      }
    }

    this.paths.push(path);

    console.log("Read path");
  }

  _setVisited(x, y, path, numSteps) {
    if (!path[x]) {
      // Never visited this x co-ord; add it
      path[x] = { y: numSteps };
    } else if (!path[x][y]) {
      // Never visited this (x,y) co-ord pair; add it
      path[x][y] = numSteps;
    }

    for (let i = 0, len = this.paths.length; i < len; i++) {
      const path = this.paths[i];

      if (path[x] && path[x][y]) {
        this.intersections.push({ x, y, p1Dist: path[x][y], p2Dist: numSteps });
      }
    }
  }

  runPart1() {
    this._loadPaths();

    let closestIntersection = Infinity;

    for (let i = 0, len = this.intersections.length; i < len; i++) {
      const intersection = this.intersections[i];

      const dist = Math.abs(intersection.x) + Math.abs(intersection.y);

      if (dist < closestIntersection) {
        closestIntersection = dist;
      }
    }

    return closestIntersection;
  }

  runPart2() {
    this._loadPaths();

    let minimumIntersection = Infinity;

    for (let i = 0, len = this.intersections.length; i < len; i++) {
      const intersection = this.intersections[i];

      const numSteps = intersection.p1Dist + intersection.p2Dist;

      if (numSteps < minimumIntersection) {
        minimumIntersection = numSteps;
      }
    }

    return minimumIntersection;
  }
}

module.exports = Day3;
