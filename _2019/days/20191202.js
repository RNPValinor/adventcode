var fs = require("fs");

fs.readFile("20191202.input", (err, data) => {
  const dataArr = data.toString().split(",");

  for (let i = 0, len = dataArr.length; i < len; i += 4) {
    const opCode = parseInt(dataArr[i]);
    const a = parseInt(dataArr[dataArr[i + 1]] || 0);
    const b = parseInt(dataArr[dataArr[i + 2]] || 0);
    const resIdx = parseInt(dataArr[i + 3]);

    let exit = false;

    switch (opCode) {
      case 1:
        console.log("Opcode 1");
        dataArr[resIdx] = a + b;
        break;
      case 2:
        console.log("Opcode 2");
        dataArr[resIdx] = a * b;
        break;
      case 99:
        console.log("Opcode 99");
        exit = true;
        break;
      default:
        console.log("Bad opcode: " + opCode);
        exit = true;
        break;
    }

    if (exit) {
      break;
    }
  }

  console.log("Pos 0: " + dataArr[0]);
});
