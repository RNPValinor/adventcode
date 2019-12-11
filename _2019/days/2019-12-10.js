const BaseDay = require("./BaseDay");
const fs = require("fs");
const _ = require("lodash");
const Decimal = require("decimal.js");

class Day10 extends BaseDay {
  _getAsteroids() {
    if (!this._asteroids) {
      this._asteroids = [];

      const lines = fs
        .readFileSync("inputs/10.txt")
        .toString()
        .split("\n");

      for (let y = 0, maxY = lines.length; y < maxY; y++) {
        const line = lines[y];

        for (let x = 0, maxX = line.length; x < maxX; x++) {
          const char = line[x];

          if (char === "#") {
            // It's an asteroid!
            this._asteroids.push({ x, y });
          }
        }
      }
    }

    return this._asteroids;
  }

  _calculateAsteroidData(asteroid, asteroidList) {
    const angles = {};

    for (let i = 0, len = asteroidList.length; i < len; i++) {
      const otherAsteroid = asteroidList[i];

      if (asteroid === otherAsteroid) {
        continue;
      }

      const relX = otherAsteroid.x - asteroid.x;
      const relY = otherAsteroid.y - asteroid.y;

      // Our co-ordinates consider +y to be down, polar co-ordinates want +y to be up
      const angle = Math.atan2(-relY, relX);
      const distance = Math.sqrt(Math.pow(relX, 2) + Math.pow(relY, 2));
      const angleData = { distance, asteroid: otherAsteroid };

      if (!angles[angle]) {
        angles[angle] = [angleData];
      } else {
        angles[angle].splice(
          _.sortedIndexBy(angles[angle], angleData, "distance"),
          0,
          angleData
        );
      }
    }

    if (this.verbose) {
      console.log(
        `Found ${_.size(angles)} visible asteroids from (${asteroid.x}, ${
          asteroid.y
        })`
      );
    }

    return angles;
  }

  _getMonitoringStationLocation() {
    if (!this._monitoringStationLocation) {
      const asteroidList = this._getAsteroids();

      let maxVisible = 0;

      asteroidList.forEach(asteroid => {
        const angles = this._calculateAsteroidData(asteroid, asteroidList);
        const numVisible = _.size(angles);

        if (numVisible > maxVisible) {
          maxVisible = numVisible;
          this._monitoringStationLocation = { asteroid, angles };
        }
      });
    }

    return this._monitoringStationLocation;
  }

  runPart1() {
    const { angles } = this._getMonitoringStationLocation();

    return _.size(angles);
  }

  runPart2() {
    const { angles } = this._getMonitoringStationLocation();

    const orderedAngles = _.orderBy(_.keys(angles), a => a - 0, ["desc"]);

    let up = Math.PI / 2;
    let index = 0;

    while (orderedAngles[index] > up && index < orderedAngles.length) {
      index++;
    }

    if (index === orderedAngles.length) {
      index = 0;
    }

    let lastZapped;

    if (orderedAngles.length >= 200) {
      // 200 or more unique angles; we don't care about any asteroid except the closest for the 200th angle we check
      lastZapped =
        angles[orderedAngles[(index + 199) % orderedAngles.length]][0].asteroid;
    } else {
      let numZapped = 0;

      while (numZapped < 200) {
        const asteroids = angles[orderedAngles[index++]];

        if (asteroids.length) {
          lastZapped = asteroids.shift().asteroid;
          numZapped++;
        }

        if (index === orderedAngles.length) {
          index = 0;
        }
      }
    }

    return lastZapped.x * 100 + lastZapped.y;
  }
}

module.exports = Day10;
