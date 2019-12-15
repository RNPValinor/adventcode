const BaseDay = require("./BaseDay");
const fs = require("fs");

class Day14 extends BaseDay {
  _getReactions() {
    if (!this._reactionsByOutput) {
      const lines = fs
        .readFileSync("inputs/14.txt")
        .toString()
        .split("\n");

      this._reactionsByOutput = {};
      const unmappedInputs = {};

      for (let i = 0, len = lines.length; i < len; i++) {
        const line = lines[i];

        const [inputsString, output] = line.split(" => ");

        const [numProduced, outputType] = output.split(" ");

        const reaction = new Reaction(outputType, numProduced);

        this._reactionsByOutput[outputType] = reaction;

        const inputs = inputsString.split(", ");

        for (let j = 0, numInputs = inputs.length; j < numInputs; j++) {
          const input = inputs[j];

          const [numRequired, inputType] = input.split(" ");

          if (this._reactionsByOutput[inputType]) {
            reaction.addInput(
              this._reactionsByOutput[inputType],
              false,
              numRequired
            );
          } else if (inputType === "ORE") {
            reaction.addInput(null, true, numRequired);
          } else {
            if (!unmappedInputs[inputType]) {
              unmappedInputs[inputType] = [{ reaction, numRequired }];
            } else {
              unmappedInputs[inputType].push({ reaction, numRequired });
            }
          }
        }

        if (unmappedInputs[outputType]) {
          for (
            let i = 0, len = unmappedInputs[outputType].length;
            i < len;
            i++
          ) {
            const { reaction: otherReaction, numRequired } = unmappedInputs[
              outputType
            ][i];

            otherReaction.addInput(reaction, false, numRequired);
          }
        }
      }
    }

    const reactions = Object.values(this._reactionsByOutput);

    for (let i = 0, len = reactions.length; i < len; i++) {
      const reaction = reactions[i];
      reaction.clean();
    }

    return this._reactionsByOutput;
  }

  runPart1() {
    const reactionsByOutput = this._getReactions();

    return reactionsByOutput.FUEL.produce(1);
  }

  runPart2() {
    const reactionsByOutput = this._getReactions();

    // Given a clean system (no spare product) this is how much ore it takes to produce 1 fuel
    const maxOrePerFuel = reactionsByOutput.FUEL.produce(1);

    // We have 1000000000000 units of ore, and our max ore per fuel is as above.
    // Therefore we can produce at least 1000000000000/maxOrePerFuel fuel
    let numFuel = Math.floor(1000000000000 / maxOrePerFuel);

    // We produced 1 fuel while working out maxOrePerFuel earlier
    let numOre = reactionsByOutput.FUEL.produce(numFuel - 1);

    // While we still have spare ore, produce 1 more fuel.
    while (numOre <= 1000000000000) {
      numOre += reactionsByOutput.FUEL.produce(1);
      numFuel++;
    }

    // We ran the system 2 too many times - once at the start, once at the end
    return numFuel - 2;
  }
}

class Reaction {
  constructor(output, numProduced) {
    this.output = output;

    if (typeof numProduced === "number") {
      this.numProduced = numProduced;
    } else {
      this.numProduced = parseInt(numProduced);
    }

    this.inputs = [];

    this.spareProduct = 0;
  }

  clean() {
    this.spareProduct = 0;
  }

  /**
   * Runs this reaction, recursively calling reactions to create inputs.
   * Will return the amount of ore required.
   * @param {int} amountToProduce Amount of the output of this reaction to create
   */
  produce(amountToProduce) {
    if (amountToProduce <= this.spareProduct) {
      // Already got enough spare product on this reaction to fulfill the requirement
      this.spareProduct -= amountToProduce;
      return 0;
    } else {
      // Use the spare product first, before running the reaction again
      amountToProduce -= this.spareProduct;
      this.spareProduct = 0;
    }

    const timesToReact = Math.ceil(amountToProduce / this.numProduced);

    let numOre = 0;

    for (let i = 0, len = this.inputs.length; i < len; i++) {
      const input = this.inputs[i];

      if (input.isOre) {
        numOre += input.numRequired * timesToReact;
      } else {
        numOre += input.reaction.produce(input.numRequired * timesToReact);
      }
    }

    const totalProduced = timesToReact * this.numProduced;
    this.spareProduct = totalProduced - amountToProduce;

    return numOre;
  }

  addInput(reaction, isOre, numRequired) {
    this.inputs.push({
      isOre,
      reaction,
      numRequired:
        typeof numRequired === "number" ? numRequired : parseInt(numRequired)
    });
  }
}

module.exports = Day14;
