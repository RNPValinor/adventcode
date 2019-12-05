const getDigits = num => {
  const digits = [];

  while (num > 0) {
    // Get the last digit of the number
    const nextDigit = num % 10;

    digits.push(nextDigit);

    // Having removed the last digit remove the trailing 0
    num = (num - nextDigit) / 10;
  }

  // We've added the digits in reverse order; reverse the array to fix this
  digits.reverse();

  return digits;
};

module.exports = getDigits;
