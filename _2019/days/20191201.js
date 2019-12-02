fuelForMass = mass => {
  let totalFuel = 0;
  let lastFuel = Math.floor(mass / 3) - 2;
  do {
    totalFuel += lastFuel;
    lastFuel = Math.floor(lastFuel / 3) - 2;
  } while (lastFuel > 0);
  realTotalFuel += totalFuel;
};
