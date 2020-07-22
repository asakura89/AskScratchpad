let data = [
    { "Type": "Fruit", "Color": "Red", "Name": "Red Apples" },
    { "Type": "Fruit", "Color": "Red", "Name": "Blood Oranges" },
    { "Type": "Vegetables", "Color": "Red", "Name": "Beets" },
    { "Type": "Vegetables", "Color": "Red", "Name": "Red Peppers" },
    { "Type": "Fruit", "Color": "Yellow/Orange", "Name": "Yellow Apples" },
    { "Type": "Fruit", "Color": "Yellow/Orange", "Name": "Apricots" },
    { "Type": "Vegetables", "Color": "Yellow/Orange", "Name": "Yellow Apples" },
    { "Type": "Vegetables", "Color": "Yellow/Orange", "Name": "Apricots" },
    { "Type": "Fruit", "Color": "Blue/Purple", "Name": "Blackberries" },
    { "Type": "Fruit", "Color": "Blue/Purple", "Name": "Blueberries" },
    { "Type": "Vegetables", "Color": "Blue/Purple", "Name": "Black Olives" },
    { "Type": "Vegetables", "Color": "Blue/Purple", "Name": "Purple Asparagus" },
    { "Type": "Fruit", "Color": "White/Tan/Brown", "Name": "Bananas" },
    { "Type": "Fruit", "Color": "White/Tan/Brown", "Name": "Dates" },
    { "Type": "Vegetables", "Color": "White/Tan/Brown", "Name": "Cauliflower" },
    { "Type": "Vegetables", "Color": "White/Tan/Brown", "Name": "Garlic" },
    { "Type": "Fruit", "Color": "Green", "Name": "Avocados" },
    { "Type": "Fruit", "Color": "Green", "Name": "Green Apples" },
    { "Type": "Vegetables", "Color": "Green", "Name": "Artichokes" },
    { "Type": "Vegetables", "Color": "Green", "Name": "Arugula" }
];

data.forEach(item => console.log(item));

data
    .filter(item => item.Type === "Fruit" && item.Color === "Green")
    .map(fruit => fruit.Name)
    .forEach(name => console.log(name));

data
    .filter(item => item.Type === "Fruit");