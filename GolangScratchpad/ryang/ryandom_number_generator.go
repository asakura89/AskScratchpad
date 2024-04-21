package ryang

import (
    "math/rand"
    "time"
)

const feigenbaum = 46692016

type RyandomNumberGenerator struct {}

func (rng *RyandomNumberGenerator) Ryandomize(lowerBound, upperBound int32) int32 {
    seed := time.Now().UnixNano() % feigenbaum
    rnd := rand.New(rand.NewSource(seed))
    return lowerBound + rnd.Int31n(upperBound-lowerBound)
}

func (rng *RyandomNumberGenerator) RyandomizeUpperBound(upperBound int32) int32 {
    return rng.Ryandomize(0, upperBound)
}