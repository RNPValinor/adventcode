const yargs = require("yargs");

function processArguments() {
  return yargs
    .usage("Usage: node index.js <command> [arguments...]")
    .command(
      "solve",
      "Solves either the current day (default) or a specified day",
      yargs => {
        return yargs
          .option("date", {
            demandOption: false,
            describe: "Date to solve in YYYY-MM-DD format",
            type: "string"
          })
          .option("part1", {
            demandOption: false,
            describe: "Solve part 1",
            default: true,
            type: "boolean"
          })
          .option("part2", {
            demandOption: false,
            describe: "Solve part 2",
            default: true,
            type: "boolean"
          });
      }
    )
    .help("h").argv;
}

function index() {
  const argv = processArguments();

  let solutionName;

  if (argv.date) {
    solutionName = argv.date;
  } else {
    const date = new Date();
    solutionName = date.toISOString().split("T")[0];
  }

  const dayClass = require(`./days/${solutionName}`);

  if (!dayClass) {
    console.error(`Could not find solution for ${solutionName}`);
    process.exit(1);
  }

  const day = new dayClass();

  if (argv.part1) {
    console.log(`Part 1: ${day.runPart1()}`);
  }

  if (argv.part2) {
    console.log(`Part 2: ${day.runPart2()}`);
  }
}

index();
