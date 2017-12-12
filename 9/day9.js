var fs = require('fs')

fs.readFile('stream.txt', (err, data) => {
  let input = data.toString()
  let groupScore = 0
  let totalScore = 0
  let cancelledCount = 0
  let inGarbage = false

  for (let i = 0, len = input.length; i < len; i++) {
    let char = input[i]
    let isIgnoredGarbage = false

    switch (char) {
      case '{':
        if (!inGarbage) {
          groupScore++
          totalScore += groupScore
        }
        break
      case '}':
        if (!inGarbage) {
          groupScore--
        }
        break
      case '!':
        isIgnoredGarbage = true
        i++
        break
      case '<':
        if (!inGarbage) {
          isIgnoredGarbage = true
        }
        inGarbage = true
        break
      case '>':
        inGarbage = false
        break
    }

    if (!isIgnoredGarbage && inGarbage) {
      cancelledCount++
    }
  }

  console.log('Total score: ' + totalScore)
  console.log('Chars in garbage: ' + cancelledCount)
})
