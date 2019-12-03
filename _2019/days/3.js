const fs = require("fs");
const BaseDay = require("./BaseDay");

class Day3 extends BaseDay {
    constructor() {
        super()
        this.paths = []
        this.intersections = []
    }

    _loadPaths() {
        if (this._pathsLoaded) {
            return
        }

        const pathData = fs.readFileSync("inputs/3.txt").split('\n');

        
    }

    runPart1() {

    }
}

module.exports = Day3