var _ = require('lodash')
var fs = require('fs')

fs.readFile('passphrases.txt', (err, data) => {
    let result = data.toString()
    let lines = result.split('\n')

    let correct = 0
    let secure = 0

    for (let i = 0; i < lines.length; i++) {
        let line = lines[i]
        let words = line.split(' ')

        if (_.uniq(words).length === words.length) {
            correct++
        }

        sortedWords = _.map(words, (w) => { return _.sortBy(w, (c) => { return c }).join('')})

        if (_.uniq(sortedWords).length === words.length) {
            secure++
        }
    }

    console.log("Correct passphrases: " + correct)
    console.log("Secure passphrases: " + secure)
})
