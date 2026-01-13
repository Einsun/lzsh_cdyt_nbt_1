<template>
  <div class="wrapper">
    <div class="table-wrapper">
    员工信息
    </div>
  </div>
</template>

<script>
export default {
  mounted () {
  },
  data () {
    return {
    };
  },
  methods: {

  }
}
</script>

<style lang="scss" scoped>

  @import '~@/assets/style/theme/register.scss';
  .wrapper{
    &::v-deep .table-wrapper{
      @include el-table-reset-primary();
    }
    .table-wrapper {
      @include styled-scrollbar;
      position: relative;
      max-height: 700px;
      overflow-y: auto;
    }
    .handle-btn{
      margin-right: 30px;
    }
  }

</style>
