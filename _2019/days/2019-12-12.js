const BaseDay = require("./BaseDay");
const Vector3 = require("../utils/Vector3");
const fs = require("fs");
const lcm = require("../utils/lcm");

class Day12 extends BaseDay {
  _loadStartCoords() {
    const data = fs
      .readFileSync("inputs/12.txt")
      .toString()
      .split("\n");

    const startCoords = [];
    const coordMatch = /^<x=(-?[0-9]+), y=(-?[0-9]+), z=(-?[0-9]+)>$/;

    for (let i = 0, len = data.length; i < len; i++) {
      const line = data[i];

      const matches = line.match(coordMatch);

      const vec = new Vector3(
        parseInt(matches[1]),
        parseInt(matches[2]),
        parseInt(matches[3])
      );

      startCoords.push(vec);
    }

    return startCoords;
  }

  _doGravity(positions, velocities) {
    for (let i = 0, len = positions.length; i < len; i++) {
      const pos = positions[i];

      for (let j = i + 1, len = positions.length; j < len; j++) {
        const otherPos = positions[j];

        this._attract(pos, otherPos, velocities[i], velocities[j], "x");
        this._attract(pos, otherPos, velocities[i], velocities[j], "y");
        this._attract(pos, otherPos, velocities[i], velocities[j], "z");
      }
    }

    for (let i = 0, len = positions.length; i < len; i++) {
      const pos = positions[i];
      const vel = velocities[i];

      pos.translateBy(vel);
    }
  }

  _attract(pos, otherPos, vel, otherVel, coord) {
    if (pos[coord] > otherPos[coord]) {
      vel[coord] -= 1;
      otherVel[coord] += 1;
    } else if (pos[coord] < otherPos[coord]) {
      vel[coord] += 1;
      otherVel[coord] -= 1;
    }
  }

  runPart1() {
    const positions = this._loadStartCoords();
    const velocities = [];

    for (let i = 0, len = positions.length; i < len; i++) {
      velocities.push(new Vector3(0, 0, 0));
    }

    for (let i = 0; i < 1000; i++) {
      this._doGravity(positions, velocities);
    }

    let energy = 0;

    for (let i = 0, len = positions.length; i < len; i++) {
      const pos = positions[i];
      const vel = velocities[i];

      energy += pos.getMagnitude() * vel.getMagnitude();
    }

    return energy;
  }

  _isAlreadySeen(positions, velocities, coord, seen, numSteps) {
    const key = this._getKey(positions, velocities, coord);

    if (seen[key] !== undefined) {
      return true;
    } else {
      seen[key] = numSteps;
      return false;
    }
  }

  _getKey(positions, velocities, coord) {
    let key = "";

    for (let i = 0, len = positions.length; i < len; i++) {
      const pos = positions[i][coord];
      const vel = velocities[i][coord];

      key += `${pos},${vel},`;
    }

    return key;
  }

  runPart2() {
    const positions = this._loadStartCoords();
    const velocities = [];

    for (let i = 0, len = positions.length; i < len; i++) {
      velocities.push(new Vector3(0, 0, 0));
    }

    let xStart, xCycle, yStart, yCycle, zStart, zCycle;

    const seenX = {
      "0,0,0,0,0,0,": 0
    };

    const seenY = {
      "0,0,0,0,0,0,": 0
    };

    const seenZ = {
      "0,0,0,0,0,0,": 0
    };

    for (let i = 1; !xCycle || !yCycle || !zCycle; i++) {
      this._doGravity(positions, velocities);

      if (
        !xCycle &&
        this._isAlreadySeen(positions, velocities, "x", seenX, i)
      ) {
        xStart = seenX[this._getKey(positions, velocities, "x")];
        xCycle = i - xStart;
      }

      if (
        !yCycle &&
        this._isAlreadySeen(positions, velocities, "y", seenY, i)
      ) {
        yStart = seenY[this._getKey(positions, velocities, "y")];
        yCycle = i - yStart;
      }

      if (
        !zCycle &&
        this._isAlreadySeen(positions, velocities, "z", seenZ, i)
      ) {
        zStart = seenZ[this._getKey(positions, velocities, "z")];
        zCycle = i - zStart;
      }
    }

    return lcm([xCycle, yCycle, zCycle]);
  }
}

module.exports = Day12;
