var fs = require('fs')
var _ = require('lodash')
fs.readFile('stacks.txt', (err, data) => {
    let lines = data.toString().split('\r\n')
    let programs = {}
    let parents = {}

    for (let i = 0; i < lines.length; i++) {
        let info = lines[i].split(' -> ')

        let programData = info[0].split(' ')
        let name = programData[0]
        let weight = parseInt(programData[1].substring(1, programData[1].length - 1))
        console.log("Got name " + name + " and weight " + weight)

        let prog = {
            weight: parseInt(weight),
            children: {},
            name: name
        }

        if (parents[name]) {
            parents[name].children[name] = prog
            delete parents[name]
        } else {
            programs[name] = prog
        }

        if (info[1]) {
            let children = info[1].split(', ')

            for (let j = 0; j < children.length; j++) {
                let child = children[j]

                if (programs[child]) {
                    prog.children[child] = programs[child]
                    delete programs[child]
                } else {
                    parents[child] = prog
                }
            }
        }
    }

    console.log("Number of root-level programs: " + _.size(programs))
    console.log("Root programs: " + _.keys(programs).join(', '))

    let balance = (prog) => {
        console.log("Balancing " + prog.name)
        let name
        let childWeights = {}

        for (name in prog.children) {
            let child = prog.children[name]
            let weight = balance(child)

            if (!childWeights[weight]) {
                childWeights[weight] = []
            }

            childWeights[weight].push(child)
        }

        if (_.size(childWeights) > 1) {
            console.log("Got mismatch child " + prog.name + " weights: " + _.keys(childWeights).join(', '))

            let weight
            let currentWeight
            let targetWeight
            let targetChild

            for (weight in childWeights) {
                if (childWeights[weight].length === 1) {
                    currentWeight = weight
                    targetChild = childWeights[weight][0]
                } else {
                    targetWeight = weight
                }
            }

            let difference = targetWeight - currentWeight
            let newWeight = targetChild.weight + difference
            console.log("New weight for " + targetChild.name + " is " + newWeight)
            process.exit()
        } else {
            if (_.size(prog.children)) {
                let weight = _.keys(childWeights)[0]
                let balanceWeight = prog.weight + childWeights[weight].length * weight
                console.log("Prog with children " + prog.name + " returns balance " + balanceWeight)
                return balanceWeight
            } else {
                console.log("Childless program " + prog.name + " returns balance " + prog.weight)
                return prog.weight
            }
        }
    }

    balance(_.values(programs)[0])
})