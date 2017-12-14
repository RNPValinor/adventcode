var _ = require('lodash')

var knotHash = (key) => {
    let numRounds = 64
    let data = _.range(0, 256)
    let curPos = 0
    let skip = 0

    key = key.concat([17,31,73,47,23])

    while (numRounds > 0) {
        for (let i = 0; i < key.length; i++) {
            let revLength = key[i]
            let newData = _.clone(data)

            for (let j = 0; j < revLength; j++) {
                let index = (curPos + j) % data.length
                let revIndex = (curPos + revLength - 1 - j) % data.length
                newData[index] = data[revIndex]
            }

            data = newData

            curPos += revLength + skip
            skip++
        }

        numRounds--
    }

    let chunks = _.chunk(data, 16)
    let denseHash = _.map(chunks, (c) => { return _.reduceRight(c, (acc, val) => { return acc ^ val }) })

    let hexHash = _.map(denseHash, (n) => { return ('0' + n.toString(16)).substr(-2) })
    let hexString = hexHash.join('')

    return hexString
}

var getBinaryValue = (char) => {
    switch (char) {
        case '0': return '0000'
        case '1': return '0001'
        case '2': return '0010'
        case '3': return '0011'
        case '4': return '0100'
        case '5': return '0101'
        case '6': return '0110'
        case '7': return '0111'
        case '8': return '1000'
        case '9': return '1001'
        case 'a': return '1010'
        case 'b': return '1011'
        case 'c': return '1100'
        case 'd': return '1101'
        case 'e': return '1110'
        case 'f': return '1111'
    }
}

getNumUsed = (char) => {
    if (char == '0') {
        return 0
    } else if (char == '1' || char == '2' || char == '4' || char == '8') {
        return 1
    } else if (char == '3' || char == '5' || char == '6' || char == '9' || char == 'a' || char == 'c') {
        return 2
    } else if (char == 'f') {
        return 4
    } else {
        return 3
    }
}

createArray = (length) => {
    var arr = new Array(length || 0),
        i = length;

    if (arguments.length > 1) {
        var args = Array.prototype.slice.call(arguments, 1);
        while(i--) arr[length-1 - i] = createArray.apply(this, args);
    }

    return arr;
}


let input = 'xlqgujun'
let numUsed = 0
let squares = createArray(128, 128)

for (let i = 0; i < 128; i++) {
    let key = input + '-' + i
    key = _.map(key, (c) => { return c.charCodeAt(0) })

    let hash = knotHash(key)

    for (let j = 0; j < hash.length; j++) {
        numUsed += getNumUsed(hash[j])
        let binaryValue = getBinaryValue(hash[j])

        for (let k = 0; k < binaryValue.length; k++) {
            squares[i][j * 4 + k] = binaryValue[k] === '0' ? 0 : 1
        }
    }
}

console.log('Num used: ' + numUsed)

let numRegions = 0

var extractRegion = (row, column) => {
    squares[row][column] = 0

    let queue = [[row + 1, column], [row - 1, column], [row, column + 1], [row, column - 1]]

    while (queue.length > 0) {
        let pos = queue.pop()
        let x = pos[0]
        let y = pos[1]

        if (x < 0 || x >= squares.length || y < 0 || y >= squares.length) {
            continue;
        }

        if (squares[x][y] === 1) {
            squares[x][y] = 0
            queue = [[x + 1,y],[x - 1, y], [x, y + 1], [x, y - 1]].concat(queue)
        }
    }
}

for (let row = 0; row < squares.length; row++) {
    for (let column = 0; column < squares.length; column++) {
        if (squares[row][column] === 1) {
            extractRegion(row, column)
            numRegions++
        }
    }
}

console.log('Num regions: ' + numRegions)
