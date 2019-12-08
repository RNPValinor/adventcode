const BaseDay = require("./BaseDay");
const fs = require("fs");

class Day8 extends BaseDay {
  constructor(verbose) {
    super(verbose);

    this.imageWidth = 25;
    this.imageHeight = 6;
  }

  _constructImages() {
    if (!this._images) {
      this._images = [];

      const data = fs.readFileSync("inputs/8.txt").toString();

      if (data.length % (this.imageWidth * this.imageHeight) !== 0) {
        console.error("Invalid input specified");
      }

      let x = 0;
      let y = 0;
      let image;

      for (let i = 0, len = data.length; i < len; i++) {
        const pixel = data[i];

        if (x === 0 && y === 0) {
          image = new Image();
          this._images.push(image);
        }

        image.set(x, y, pixel);

        if (++x === this.imageWidth) {
          x = 0;

          if (++y === this.imageHeight) {
            y = 0;
          }
        }
      }
    }

    return this._images;
  }

  runPart1() {
    const images = this._constructImages();

    let numZeros = Infinity;
    let checksum = 0;

    for (let i = 0, len = images.length; i < len; i++) {
      const image = images[i];

      const valueCounts = image.getValueCounts();

      if (valueCounts[0] < numZeros) {
        numZeros = valueCounts[0];
        checksum = valueCounts[1] * valueCounts[2];
      }
    }

    return checksum;
  }

  runPart2() {
    const images = this._constructImages();

    let result = "\n";

    for (let y = 0; y < this.imageHeight; y++) {
      for (let x = 0; x < this.imageWidth; x++) {
        for (let i = 0, len = images.length; i < len; i++) {
          const image = images[i];
          const pixel = image.get(x, y);

          if (pixel !== "2") {
            if (pixel === "0") {
              result += "\x1b[40m ";
            } else {
              result += "\x1b[47m ";
            }
            break;
          }
        }
      }

      result += "\x1b[0m\n";
    }

    return result;
  }
}

class Image {
  constructor() {
    this.data = [];
    this.maxValue = 0;
  }

  set(x, y, val) {
    if (!this.data[x]) {
      this.data[x] = [];
    }

    this.data[x][y] = val;

    this.maxValue = Math.max(this.maxValue, val);
  }

  get(x, y) {
    if (!this.data[x]) {
      return;
    }

    return this.data[x][y];
  }

  getValueCounts() {
    const valueCounts = Array(this.maxValue + 1).fill(0);

    for (let x = 0, lenX = this.data.length; x < lenX; x++) {
      const row = this.data[x];

      for (let y = 0, lenY = row.length; y < lenY; y++) {
        const pixel = row[y];

        valueCounts[pixel]++;
      }
    }

    return valueCounts;
  }
}

module.exports = Day8;
