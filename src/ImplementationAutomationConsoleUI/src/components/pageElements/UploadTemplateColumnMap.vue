<template>
    <div>
        <div class="row">
            <div class="col-md-6">
                <h3>Column 1</h3>
                <ul class="list-group">
                    <li class="list-group-item" v-for="item in column1" :key="item" @click="selectItem(item)"
                        :class="{ 'active': selectedItem1 === item }">
                        {{ item}}
                    </li>
                </ul>
            </div>
            <div class="col-md-6">
                <h3>Column 2</h3>
                <ul class="list-group">
                    <li class="list-group-item" v-for="item in column2" :key="item.id" @click="selectItem2(item)"
                        :class="{ 'active': selectedItem2 === item }">
                        {{ item.name }}
                    </li>
                </ul>
            </div>
        </div>
        <div class="row ml-auto">
            <div class="col-md-4">
                <h3>Selected Items</h3>
                <button class="btn btn-primary" @click="pushItem()">Add</button>
                <ul class="list-group">
                    <li class="list-group-item" v-for="item in selectedItems" :key="item.id">
                        {{ item.a }} {{ item.b }}
                    </li>
                </ul>
            </div>
        </div>
    </div>
</template>
  
<script>
import { useMyStore } from '../../store';
export default {
    data() {
        return {
            selectedItem1: null,
            selectedItem2: null,
            col1Data: null,
            col2Data: null,
            colMapData: [],
            column1: [
                { id: 1, name: 'Item 1' },
                { id: 2, name: 'Item 2' },
                { id: 3, name: 'Item 3' },
                { id: 4, name: 'Item 4' },
                { id: 5, name: 'Item 5' }
            ],
            column2: [{ id: 11, name: 'Item 6' },
            { id: 6, name: 'Item 7' },
            { id: 7, name: 'Item 8' },
            { id: 8, name: 'Item 9' },
            { id: 9, name: 'Item 10' }],
            selectedItems: []
        };
    },
    methods: {
        selectItem(item) {
            this.col1Data = item;
            this.selectedItem1 = item
        },
        selectItem2(item) {
            this.col2Data = item.name;
            this.selectedItem2 = item
        },
        pushItem() {
            if (this.col1Data != null && this.col2Data != null) {
                this.selectedItems.push({ 'a': this.col1Data, 'b': this.col2Data });

                for (var i = 0; i < this.column1.length; i++) {
                    if (this.column1[i] === this.col1Data) {
                        this.column1.splice(i, 1);
                        break;
                    }
                }

                for (var j = 0; j < this.column2.length; j++) {
                    if (this.column2[j].name === this.col2Data) {
                        this.column2.splice(j, 1);
                        break;
                    }
                }
                this.col1Data = null;
                this.col2Data = null;
            }
            else {
                return;
            }
        },
    },

    mounted() {
        const store = useMyStore();
        this.column1=store.data;
    }
};
</script>
  