const BaseDay = require("./BaseDay");
const fs = require("fs");
const Queue = require("../utils/Queue");

class Day6 extends BaseDay {
  constructor(verbose, benchmark) {
    super(verbose);
    this.benchmark = benchmark;
  }

  _getPlanets() {
    if (!this._planets) {
      const data = fs
        .readFileSync(this.benchmark ? "inputs/6benchmark.txt" : "inputs/6.txt")
        .toString()
        .split("\n");

      this._planets = {};

      for (let i = 0, len = data.length; i < len; i++) {
        const line = data[i];

        const [planetId, orbiterId] = line.split(")");

        let planet = this._planets[planetId];

        if (!planet) {
          planet = new Planet(planetId);
          this._planets[planetId] = planet;
        }

        let orbiter = this._planets[orbiterId];

        if (!orbiter) {
          orbiter = new Planet(orbiterId, planet);
          this._planets[orbiterId] = orbiter;
        } else {
          orbiter.parent = planet;
        }

        planet.addOrbiter(orbiter);
      }
    }

    return this._planets;
  }

  _getNumOrbits(planet) {
    let numOrbits = 0;

    const queue = new Queue();
    queue.enqueue({ planet, numIndirect: 0 });

    while (queue.hasMore()) {
      const { planet, numIndirect } = queue.dequeue();

      // Number of orbits is the number of orbits to get to the 'parent' of this
      // orbiter (indirect), plus 1 (direct)
      numOrbits += numIndirect + 1;

      for (let i = 0, len = planet.orbiters.length; i < len; i++) {
        const orbiter = planet.orbiters[i];
        queue.enqueue({ planet: orbiter, numIndirect: numIndirect + 1 });
      }
    }

    return numOrbits;
  }

  runPart1() {
    const planets = this._getPlanets();

    const com = planets.COM;

    return this._getNumOrbits(com);
  }

  _getNumTransfers(source, targetId) {
    const seen = {};
    seen[source.id] = true;

    const queue = new Queue();
    queue.enqueue({
      planet: source.parent,
      numTransfers: 0
    });

    while (queue.hasMore()) {
      const { planet, numTransfers } = queue.dequeue();

      seen[planet.id] = true;

      if (planet.hasOrbiter(targetId)) {
        return numTransfers;
      }

      // Try and transfer to the planet this planet is orbiting
      if (planet.parent && !seen[planet.parent.id]) {
        queue.enqueue({
          planet: planet.parent,
          numTransfers: numTransfers + 1
        });
      }

      // Try and transfer to one of the other planets orbiting this one
      for (let i = 0, len = planet.orbiters.length; i < len; i++) {
        const orbiter = planet.orbiters[i];

        if (!seen[orbiter.id]) {
          queue.enqueue({ planet: orbiter, numTransfers: numTransfers + 1 });
        }
      }
    }

    return "No route found!";
  }

  runPart2() {
    const planets = this._getPlanets();

    return this._getNumTransfers(planets.YOU, "SAN");
  }
}

class Planet {
  constructor(id, parent) {
    this.id = id;
    this.parent = parent;
    this.orbiters = [];
    this.orbitingIds = {};
  }

  addOrbiter(orbiter) {
    this.orbiters.push(orbiter);
    this.orbitingIds[orbiter.id] = true;
  }

  hasOrbiter(orbiterId) {
    return this.orbitingIds[orbiterId];
  }

  setParent(parent) {
    this.parent = parent;
  }
}

module.exports = Day6;
