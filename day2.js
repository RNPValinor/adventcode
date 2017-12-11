var Papa = require('papaparse')
var fs = require('fs')
var _ = require('lodash')

fs.readFile('spreadsheet.csv', (err, data) => {
    let result = Papa.parse(data.toString())

    // Checksum calculation
    let checksum = 0

    for (let i = 0; i < result.data.length; i++) {
        let row = result.data[i]

        let min = max = +row[0]

        for (let j = 1; j < row.length; j++) {
            let val = +row[j]

            if (val > max) {
                max = val
            } else if (val < min) {
                min = val
            }
        }

        console.log("Row " + i + " got min, max: " + min + ", " + max)

        checksum += (max - min)
    }

    console.log("Checksum: " + checksum)

    // Evenly divisible calculation
    let evenDiv = 0

    _.map(result.data, (row) => {
        outer:
        for (let i = 0; i < row.length; i++) {
            inner:
            for (let j = 0; j < row.length; j++) {
                if (i == j) {
                    continue;
                }

                if (row[i] % row[j] === 0) {
                    console.log("Found evenDiv: " + row[i] + " / " + row[j])
                    evenDiv += row[i] / row[j]
                    break outer
                }
            }
        }
    })

    console.log("EvenDiv: " + evenDiv)
})