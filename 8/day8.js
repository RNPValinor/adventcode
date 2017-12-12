var fs = require('fs')
var _ = require('lodash')

var registers = {}
var maxRegValue = 0

let getRegister = (key) => {
    if (!registers[key]) {
        registers[key] = 0
    }

    return registers[key]
}

let processInstruction = (data) => {
    let shouldRun = false
    let compVal = getRegister(data.condReg)

    switch (data.condType) {
        case ">":
            shouldRun = compVal > data.condValue
            break;
        case ">=":
            shouldRun = compVal >= data.condValue
            break;
        case "<":
            shouldRun = compVal < data.condValue
            break;
        case "<=":
            shouldRun = compVal <= data.condValue
            break;
        case "==":
            shouldRun = compVal == data.condValue
            break;
        case "!=":
            shouldRun = compVal != data.condValue
            break;
    }

    if (!shouldRun) {
        return
    }

    getRegister(data.register)

    if (data.command === "inc") {
        registers[data.register] += data.value
    } else {
        registers[data.register] -= data.value
    }

    maxRegValue = Math.max(maxRegValue, registers[data.register])
}

fs.readFile('instructions.txt', (err, data) => {
    let instructions = data.toString().split('\r\n')

    for (let i = 0; i < instructions.length; i++) {
        let instruction = instructions[i].split(' ')

        let instData = {
            register: instruction[0],
            command: instruction[1],
            value: parseInt(instruction[2]),
            condReg: instruction[4],
            condType: instruction[5],
            condValue: parseInt(instruction[6])
        }

        processInstruction(instData)
    }

    console.log("Max register value: " + _.max(_.values(registers)))
    console.log("Max seen value: " + maxRegValue)
})