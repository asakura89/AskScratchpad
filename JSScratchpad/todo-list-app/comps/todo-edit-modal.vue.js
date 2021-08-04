const template = `
    <div id="add-task-modal" class="modal fade" tabindex="-1" role="dialog">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span
                            aria-hidden="true">&times;</span></button>
                    <h4 class="modal-title">Modal title</h4>
                </div>
                <div class="modal-body">
                    <p>One fine body&hellip;</p>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
                    <button type="button" class="btn btn-primary">Save changes</button>
                </div>
            </div>
        </div>
        </div>

        <!--<div class="modal-card" style="width: auto">
        <header class="modal-card-head">
            <p class="modal-card-title">Edit {{ todo.todo }}</p>
        </header>
        <section class="modal-card-body">
            <b-field label="Title">
                <b-input type="text" v-model="title" placeholder="Your todo title">
                </b-input>
            </b-field>

            <b-field label="Priority">
                <b-select placeholder="Select a priority" v-model="priority">
                    <option v-for="option in priorities" :value="option.name" :key="option.id">
                        {{ option.name }}
                    </option>
                </b-select>
            </b-field>
        </section>
        <footer class="modal-card-foot">
            <button class="button" type="button" @click="$parent.close()">
                Close
            </button>
            <button class="button is-primary" @click="editTodo">Save</button>
    </footer>
    </div>-->
`;

const todoEditModal = {
    name: "TodoEditModal",
    props: /*{
        todo: {
            type: Object,
            required: true
        },
        priorities: {
            type: Array,
            required: true
        }
    },*/
    [
        "EventBus"
    ],
    data() {
        /* return {
            title: "",
            priority: "",
            isHidden: true,
            isVisible: false,
            isTransitioning: false,
            isShow: false,
            isOpening: false,
            isClosing: false
        } */
        return {};
    },
    template: template,
    created() {
        //this.$root.$refs.EditModal = this;

    },
    mounted() {
        /* this.title = this.todo.todo;
        this.priority = this.todo.priority; */

        this.EventBus.$on("TodoEditModal:Show", this.show)
    },
    beforeDestroy() {
        /* if (this.isVisible) {
            this.isVisible = false;
            this.isShow = false;
            this.isTransitioning = false;
        } */
    },
    methods: {
        /* editTodo() {
            const payload = {
                id: this.todo.id,
                todo: this.title,
                priority: this.priority
            };
            this.$emit("edit-todo", payload);
        }, */
        show() {
            /* if (this.isVisible || this.isOpening) {
                return;
            }

            if (this.isClosing) {
                this.$once("hidden", this.Show);
                return;
            }

            this.isOpening = true; */
            jQuery.modal("#add-task-modal");
        }
    }
};

export default todoEditModal;