const Days = require("./days/Days");

const date = new Date();

const day = Days[date.getDate()];

console.log(`Part 1: ${day.runPart1()}`);

console.log(`Part 2: ${day.runPart2()}`);
