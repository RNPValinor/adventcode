const BaseDay = require("./BaseDay");
const Vector3 = require("../utils/Vector3");
const fs = require("fs");

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

  runPart2() {}
}

module.exports = Day12;
