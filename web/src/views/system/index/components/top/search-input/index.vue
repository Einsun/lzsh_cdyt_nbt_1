<template>
  <el-autocomplete
    v-model="value"
    placeholder="请输入关键词…"
    clearable
    :fetch-suggestions="querySearch"
    @select="handleSelect"
  ></el-autocomplete>
</template>

<script>
export default {
  name: 'search-input',
  data () {
    return {
      value: ''
    }
  },
  computed: {
    queryCompanyList () {
      return this.$store.state.sinuo.user.companyList.map(item => {
        return {
          ...item,
          value: item.unitName
        }
      })
    }
  },
  methods: {
    querySearch (queryString, cb) {
      const restaurants = this.queryCompanyList;
      const results = queryString ? restaurants.filter(item => {
        return item.value.toLowerCase().includes(queryString ? queryString.toLowerCase() : '')
      }) : restaurants;
      // 调用 callback 返回建议列表的数据
      cb(results);
    },
    handleSelect (item) {
      this.$store.commit('sinuo/dashboard/setMapActiveUnit', item)
    }
  }
}
</script>

<style scoped lang="scss">
.el-autocomplete::v-deep{
  .el-input{
    width: auto;
  }
  .el-input__inner{
    width:220px;
    height:36px;
    background:rgba(6,39,91,.8);
    border-radius:19px;
    opacity:0.8;
    border:1px solid rgba(46,192,243,1);
    color:rgba(46,192,243,1);
    transition: width .5s ease-in-out;
    transform: translate3d(0,0,0);

    &::-webkit-input-placeholder {
      color:rgba(46,192,243,.5);
    }
    &:-moz-placeholder {/* Firefox 18- */
      color:rgba(46,192,243,.5);
    }
    &::-moz-placeholder{/* Firefox 19+ */
      color:rgba(46,192,243,.5);
    }
    &:-ms-input-placeholder {
      color:rgba(46,192,243,.5);
    }

    /*&:focus{*/
    /*  width: 470px;*/
    /*}*/
  }
  .el-input__suffix{
    color: #2EC0F3;
    padding-right: 3px;
    .el-input__suffix-inner{
      position: relative;
      top: -3px;
    }
  }
}
</style>
