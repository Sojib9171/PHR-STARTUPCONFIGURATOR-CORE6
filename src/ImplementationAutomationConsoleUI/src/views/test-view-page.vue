<template>
  <div class="row">
    <div class="col-4">
      <h3>Draggable {{ draggingInfo }}</h3>

      <draggable
        :list="list"
        item-key="name"
        class="list-group"
        ghost-class="ghost"
        :move="checkMove"
        @start="dragging = true"
        @end="dragging = false"
      >
      <template #item="{ element, index }">
          <div class="list-group-item">
            <span class="custom-ranking-number">{{ index + 1 }}</span>
            {{ element.name }}
          </div>
        </template>
      </draggable>
    </div>

    <rawDisplayer class="col-3" :value="list" title="List" />
  </div>
</template>

<script>
import draggable from "../../node_modules/vuedraggable";
export default {
  order: 0,
  components: {
    draggable
  },
  data() {
    return {
      list: [
        { name: "John", id: 0, order:1 },
        { name: "Joao", id: 1, order:2 },
        { name: "Jean", id: 2, order:3 }
      ],
      dragging: false
    };
  },
  methods: {
    checkMove: function(e) {
      e.draggedContext.element=e.draggedContext.futureIndex;
    }
  }
};
</script>
<style scoped>
.buttons {
  margin-top: 35px;
}

.ghost {
  opacity: 0.5;
  background: #c8ebfb;
}

.not-draggable {
  cursor: no-drop;
}

.custom-ranking-number {
    background-color: #3498db;
    color: #fff;
    padding: 5px 10px;
    border-radius: 5px;
    margin-right: 10px;
  }
</style>