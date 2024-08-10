package main

/*
import (
    "askscratchpad/golang/ryang"
    "fmt"
)

func main() {
    rng := ryang.RyandomNumberGenerator{}

    fmt.Println(rng.Ryandomize(5, 10))
    fmt.Println(rng.RyandomizeUpperBound(10))
}
*/

/*
import (
    "fmt"
)

func main() {
    a := A{}
    angka := a.SomeFunc()
    fmt.Println(angka)
}

type A struct {}

func (a *A) SomeFunc() (int32) {
    return 46692016
}
*/

import (
    "fmt"
)

type temp struct {
    TempPropA string
    TempPropB int
}

func main() {
    b := New() //B{}
    keyExist := b.ContainsKey("temp")
    val := b.Get("temp")

    fmt.Println(keyExist)
    fmt.Println(val)

    _ = b.Add("temp", &temp {
        TempPropA: "hello",
        TempPropB: 8,
    })

    keyExist = b.ContainsKey("temp")
    val = b.Get("temp")

    fmt.Println(keyExist)
    fmt.Println(val)
}

type B struct {
    internalStorage map[string]interface{}
}

func New() *B {
    return &B {
        internalStorage: make(map[string]interface{}),
    }
}

func (b *B) Add(key string, value interface{}) (bool) {
    if b.ContainsKey(key) {
        return false
    }

    b.internalStorage[key] = value
    return true
}

func (b *B) Get(key string) (interface{}) {
    if !b.ContainsKey(key) {
        return nil
    }

    return b.internalStorage[key]
}

func (b *B) ContainsKey(key string) (bool) {
    _, ok := b.internalStorage[key]
    return ok
}
