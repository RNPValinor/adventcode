const BaseDay = require("./BaseDay");
const fs = require("fs");

class Day16 extends BaseDay {
  _loadInput() {
    const data = fs.readFileSync("inputs/16.txt").toString();
    const input = [];

    for (let i = 0, len = data.length; i < len; i++) {
      input.push(parseInt(data[i]));
    }

    return input;
  }

  runPart1() {
    const fft = [0, 1, 0, -1];
    let numbers = this._loadInput();

    for (let phase = 0; phase < 100; phase++) {
      let nextNumbers = [];

      for (let i = 0, len = numbers.length; i < len; i++) {
        let nextNumber = 0;

        for (let j = 0, lenJ = numbers.length; j < lenJ; j++) {
          const input = numbers[j];

          const fftTransform = fft[Math.floor((j + 1) / (i + 1)) % 4];

          nextNumber += fftTransform * input;
        }

        nextNumbers.push(Math.abs(nextNumber % 10));
      }

      numbers = nextNumbers;
    }

    return numbers.slice(0, 8).join("");
  }

  runPart2() {
    const fft = [0, 1, 0, -1];
    let numbers = this._loadInput();
    const offset = parseInt(numbers.slice(0, 7).join(""));
    let numNumbers = numbers.length * 10000;

    let result = [];

    for (let i = 0; i < 7; i++) {
      result.push(
        this._getValueAfterNPhases(
          offset + i,
          100,
          numNumbers,
          numbers,
          fft,
          {}
        )
      );
    }

    return result.join("");
  }

  _getValueAfterNPhases(
    pos,
    phases,
    numNumbers,
    initialNumbers,
    fft,
    knownValues
  ) {
    if (phases === 0) {
      return initialNumbers[pos % initialNumbers.length];
    }

    if (knownValues[phases]) {
      if (knownValues[phases][pos]) {
        return knownValues[phases][pos];
      }
    }

    let result;

    for (let i = 0; i < numNumbers; i++) {
      const fftTransform = fft[Math.floor((i + 1) / (pos + 1)) % 4];

      if (fftTransform !== 0) {
        result +=
          fftTransform *
          this._getValueAfterNPhases(
            i,
            phases - 1,
            numNumbers,
            initialNumbers,
            fft,
            knownValues
          );
      }
    }

    if (!knownValues[phases]) {
      knownValues[phases] = {};
    }

    knownValues[phases][pos] = result;

    return result;
  }
}

module.exports = Day16;
