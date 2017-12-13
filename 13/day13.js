var fs = require('fs')
var _ = require('lodash')

var getCost = (picosecond, layer) => {
    let {depth, position} = layer

    if (picosecond % (2 * depth - 2) === 0) {
        return position * depth
    } else {
        return 0
    }
}

fs.readFile('layers.txt', (err, data) => {
    let lines = data.toString().split('\r\n')
    let layers = []
    let lastIndex = 0
    let computedCost = 0
    
    for (let i = 0; i < lines.length; i++) {
        let data = lines[i].split(': ')
        let index = parseInt(data[0])
        let depth = parseInt(data[1])

        layers[index] = {
            position: index,
            depth: depth
        }

        if (index % (2 * depth - 2) === 0) {
            computedCost += index * depth
        }

        while (lastIndex < index) {
            layers[lastIndex] = false
            lastIndex++
        }

        lastIndex = index + 1
    }

    console.log('Got ' + layers.length + ' layers')

    let picosecond = 0
    let curPos = 0
    let cost = 0

    while (curPos < layers.length) {
        if (layers[curPos]) {
            // Check if I just moved into a scanner
            let layer = layers[curPos]
            cost += getCost(picosecond, layer)
        }
        curPos++
        picosecond++
    }

    console.log('Total cost: ' + cost)
    console.log('Computed cost: ' + computedCost)

    let delay = 0
    let caught = false

    do {
        let picosecond = delay
        let curPos = 0
        caught = false

        while (curPos < layers.length) {
            if (layers[curPos]) {
                // Check if I just moved into a scanner
                let layer = layers[curPos]
                if (picosecond % (2 * layer.depth - 2) === 0) {
                    caught = true
                }
            }
            curPos++
            picosecond++
        }

        delay++
    } while (caught)

    console.log('Need to delay by ' + (delay - 1) + ' picoseconds')
})