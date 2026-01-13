<template>
  <div class="dash-card-wrapper">
    <dashCard title="消防安全指数排名" :decorationNumber="16">
      <template v-slot:title>
        <span @click="toggleSort">
          <i :class="sortIconClass" ></i>
        </span>
      </template>
      <div class="content" @click="handleclick($event)">
        <vue-seamless-scroll :data="rankingList" class="seamless-warp" :class-option="classOption">
          <section v-for="(item, index) in rankingList" :key="item.id" class="flex-justify-main ranking-item">
            <section class="text-wrapper flex-justify-main flex-center-cross" :data-unit="item.id" :data-label="item.name">
              <p class="label overflow" :title="item.label" :data-unit="item.id" :data-label="item.name">{{index+1}}.{{item.label.slice(0,16)}}</p>
              <hr class="dotted-line" :data-unit="item.id" :data-label="item.name">
              <p class="score-value color-primary" :data-unit="item.id" :data-label="item.name">安全指数:<span class="score-number">{{item.score}}</span></p>
            </section>
          </section>
        </vue-seamless-scroll>
      </div>
    </dashCard>
  </div>
</template>

<script>
import dashCard from '../../card/index'
import vueSeamlessScroll from 'vue-seamless-scroll'

export default {
  name: 'fire-control-index',
  components: {
    dashCard,
    vueSeamlessScroll
  },
  activated () {
  },
  data () {
    return {
      sort: 'desc'
    };
  },
  methods: {
    toggleSort () {
      if (this.sort === 'desc') {
        this.sort = 'asc'
      } else {
        this.sort = 'desc'
      }
    },
    handleclick (event) {
      let id = event.target.dataset.unit
      let name = event.target.dataset.label
      this.$router.push({
        name: 'reportList',
        params: {
          id: id,
          name: name
        }
      })
    }
  },
  computed: {
    classOption () {
      return {
        step: 0.2, // 数值越大速度滚动越快
        limitMoveNum: 2, // 开始无缝滚动的数据量 this.dataList.length
        hoverStop: true, // 是否开启鼠标悬停stop
        direction: 1, // 0向下 1向上 2向左 3向右
        openWatch: true, // 开启数据实时监控刷新dom
        singleHeight: 0, // 单步运动停止的高度(默认值0是无缝不停止的滚动) direction => 0/1
        singleWidth: 0, // 单步运动停止的宽度(默认值0是无缝不停止的滚动) direction => 2/3
        waitTime: 1000 // 单步运动停止的时间(默认值1000ms)
      }
    },
    rankingList () {
      const list = this.$store.state.sinuo.user.companyList
      const rankingList = list.map(item => {
        return {
          id: item.unitID,
          name: item.unitName,
          label: item.unitName,
          score: Number.parseFloat(item.score).toFixed(2)
        }
      }).sort((a, b) => {
        let result
        if (this.sort === 'desc') {
          result = b.score - a.score
        } else {
          result = a.score - b.score
        }
        return result
      })
      var res = rankingList.slice(0, 50)
      return res
    },
    sortIconClass () {
      return {
        'el-icon-sort-up': this.sort === 'asc',
        'el-icon-sort-down': this.sort === 'desc',
      }
    }
  },
}
</script>

<style scoped lang="scss">
.dash-card-wrapper{
  height: 277px;
  &::v-deep{
    .content-wrapper{
      padding: 20px 0 0;
    }
  }
  .ranking-item{
    padding-left: 20px;
    margin-bottom: 10px;
    font-size: 14px;
    height: 14px;
    line-height: 14px;
    .text-wrapper{
      width: 371px;
      .label{
        font-weight:400;
        color:rgba(245,246,245,1);
        max-width: 14em;
        /*overflow: hidden;*/
        white-space: nowrap;
        /*text-overflow:ellipsis;*/
      }
      .dotted-line{
        flex: 1;
        margin: 0 3px 0 10px;
        height: 1px;
        border:1px dashed rgba(255,255,255,.3);
      }
      .ranking-number{
        margin-left: 10px;
      }
    }
  }
}
</style>
