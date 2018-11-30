var fs = require('fs')
var _ = require('lodash')

fs.readFile('jumps.txt', (err, data) => {
    let lines = data.toString().split('\n')

    let jumps = _.map(lines, (j) => { return parseInt(j) })

    let index = 0
    let numJumps = 0

    while (index < jumps.length) {
        let oldIndex = index
        index += jumps[index]

        if (jumps[oldIndex] >= 3) {
            jumps[oldIndex]--
        } else {
            jumps[oldIndex]++
        }
        
        numJumps++  
    }

    console.log("Jumps: " + numJumps)
})