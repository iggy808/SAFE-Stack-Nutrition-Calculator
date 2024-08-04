module Calculations

let calculateBasalMetabolicRate weight_lb height_in age =
    (weight_lb * 4.536) + (height_in * 15.88) - (5.0 * age) + 5.0

let convertPoundsToKilograms weight_lb = ((weight_lb / 2.205) * 100.0) / 100.0

let convertInchesToCentimeters height_in = height_in * 2.54