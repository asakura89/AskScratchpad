// Import stylesheets
//import './style.css';
//import Vue from "./node_modules/vue/dist/vue";

// Write Javascript code!
//const appDiv = document.getElementById('app');
//appDiv.innerHTML = `<h1>JS Starter</h1>`;

/*
var testapp = new Vue({
  el: "#app",
  data: {
    message: "Hello Vue!"
  }
});
*/

import {default as TodoEditModal} from "./comps/todo-edit-modal.vue.js";

Vue.use(uiv);

// localStorage.clear();
const presetValues = [
    {
        title: "Wake up at 5am",
        completed: true,
    },
    {
        title: "Learn how to use Vue.js",
        completed: false,
    },
    {
        title: "Drink coffee",
        completed: false,
    },
];
// Use localStorage
const STORAGE_KEY = "todo-app";
const todoStorage = {
    fetch: function () {
        var todos =
            JSON.parse(localStorage.getItem(STORAGE_KEY)) || presetValues;
        todos.forEach(function (todo, index) {
            todo.id = index;
        });
        todoStorage.uid = todos.length;
        return todos;
    },
    save: function (todos) {
        localStorage.setItem(STORAGE_KEY, JSON.stringify(todos));
    },
};

// visibility filters
var filters = {
    all: function (todos) {
        return todos;
    },
    active: function (todos) {
        return todos.filter(function (todo) {
            return !todo.completed;
        });
    },
    completed: function (todos) {
        return todos.filter(function (todo) {
            return todo.completed;
        });
    },
};

var app = new Vue({
    data: {
        todos: todoStorage.fetch(),
        newTodo: "",
        editedTodo: null,
        visibility: "all",
        EventBus: new Vue()
    },

    // watch todos change for localStorage persistence
    watch: {
        todos: {
            handler: function (todos) {
                todoStorage.save(todos);
            },
            deep: true,
        },
    },

    // computed properties
    // http://vuejs.org/guide/computed.html
    computed: {
        filteredTodos: function () {
            return filters[this.visibility](this.todos);
        },
        remaining: function () {
            return filters.active(this.todos).length;
        },
        allDone: {
            get: function () {
                return this.remaining === 0;
            },
            set: function (value) {
                this.todos.forEach(function (todo) {
                    todo.completed = value;
                });
            },
        },
    },

    filters: {
        pluralize: function (n) {
            return n === 1 ? "task" : "tasks";
        },
    },

    components: {
        "TodoEditModal": TodoEditModal
    },

    methods: {
        addTodo: function () {
            var value = this.newTodo && this.newTodo.trim();
            if (!value) {
                return;
            }
            this.todos.push({
                id: todoStorage.uid++,
                title: value,
                completed: false,
            });
            this.newTodo = "";
        },

        removeTodo: function (todo) {
            this.todos.splice(this.todos.indexOf(todo), 1);
        },

        editTodo: function (todo) {
            this.beforeEditCache = todo.title;
            this.editedTodo = todo;
        },

        doneEdit: function (todo) {
            if (!this.editedTodo) {
                return;
            }
            this.editedTodo = null;
            todo.title = todo.title.trim();
            if (!todo.title) {
                this.removeTodo(todo);
            }
        },

        cancelEdit: function (todo) {
            this.editedTodo = null;
            todo.title = this.beforeEditCache;
        },

        removeCompleted: function () {
            this.todos = filters.active(this.todos);
        },

        OpenAddTaskModal: function () {
            //this.$root.$refs.EditModal.show();
            //TodoEditModal.show();
            this.$emit("TodoEditModal:Show");
        },
    },

    // a custom directive to wait for the DOM to be updated
    // before focusing on the input field.
    // http://vuejs.org/guide/custom-directive.html
    directives: {
        "todo-focus": function (el, binding) {
            if (binding.value) {
                el.focus();
            }
        },
    },
});

// handle routing
function onHashChange() {
    var visibility = window.location.hash.replace(/#\/?/, "");
    if (filters[visibility]) {
        app.visibility = visibility;
    } else {
        window.location.hash = "";
        app.visibility = "all";
    }
}

window.addEventListener("hashchange", onHashChange);
onHashChange();

// mount
app.$mount(".todoapp");
