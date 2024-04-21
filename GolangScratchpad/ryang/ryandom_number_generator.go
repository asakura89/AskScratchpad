package ryang

import (
    "math/rand"
    "time"
)

const feigenbaum = 46692016

func Ryandomize(lowerBound, upperBound int32) int32 {
    seed := time.Now().UnixNano() % feigenbaum
    rnd := rand.New(rand.NewSource(seed))
    return lowerBound + rnd.Int31n(upperBound-lowerBound)
}

func RyandomizeUpperBound(upperBound int32) int32 {
    return Ryandomize(0, upperBound)
}