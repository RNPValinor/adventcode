var _ = require('lodash')

let hasBeenSeen = (mem) => {
    let hash = mem.join('-')
    if (seen[hash]) {
        loopSize = numSteps - seen[hash]
        return true
    }
    seen[hash] = numSteps
    return false
}

let reallocate = (mem) => {
    let max = _.max(mem)
    let index = _.indexOf(mem, max)
    let length = mem.length

    let num = mem[index]
    mem[index] = 0
    
    while (num > 0) {
        index = (index + 1) % length
        mem[index]++
        num--
    }
}

let mem = [0,5,10,0,11,14,13,4,11,8,8,7,1,4,12,11]

var seen = {}
var loopSize = 0
var numSteps = 0

do {
    reallocate(mem)
    numSteps++
} while (!hasBeenSeen(mem, seen))

console.log("Duplication after " + numSteps + " steps")
console.log("Loop size is " + loopSize)