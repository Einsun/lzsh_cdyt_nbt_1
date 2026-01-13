<template>
  <div class="dash-card-wrapper">
    <dashCard :title="cardTitle" :decorationNumber="needDecorateLineLength">
      <div class="content">
        <!-- <el-carousel height="343px" indicator-position="none" @change="carouselChange" :interval="100000" ref="carousel">
          <el-carousel-item v-for="(item, index) in companyVideoList" :key="item.unitID">
            <video
              :id="`unit-video-${index}`"
              class="unit-video"
              autoplay
              controls
              playsInline
              webkit-playsinline
            >
              <source :src="item.SmoothsrcP" />
            </video>
          </el-carousel-item>
        </el-carousel> -->
        <el-carousel height="343px" indicator-position="none" @change="carouselChange" :interval="100000" ref="carousel">
          <el-carousel-item v-for="(item, index) in companyVideoList" :key="item.unitID">

            <video-player
              :id="`unit-video-${index}`"
              class="video-player vjs-custom-skin"
              ref="videoPlayer"
              :playsinline="true"
              :options="playerOptions"
            ></video-player>

          </el-carousel-item>
        </el-carousel>
        <sn-form :formList="unitVideoConfig" :formValue="unitVideoValue" :form-style="{}" class="form-wrapper"></sn-form>
      </div>
    </dashCard>
  </div>
</template>

<script>
import dashCard from '../../card/index'
import * as EZUIKit from '@/plugin/ezuikit/ezuikit.js'
import 'videojs-contrib-hls'

export default {
  name: 'unit-video',
  components: {
    dashCard
  },
  data () {
    const vm = this
    return {
      playerOptions: {
        playbackRates: [0.7, 1.0, 1.5, 2.0],
        autoplay: false,
        muted: false,
        loop: false,
        preload: 'auto',
        language: 'zh-CN',
        aspectRatio: '16:9',
        fluid: true,
        sources: [{
          type: "application/x-mpegURL",
          src: ""
        }],
        poster: "",
        notSupportedMessage: '此视频暂无法播放，请稍后再试',
        controlBar: {
          timeDivider: true,
          durationDisplay: true,
          remainingTimeDisplay: false,
          fullscreenToggle: true
        }
      },
      activeIndex: 0,
      unitVideoConfig: [
        {
          type: 'select',
          label: '单位',
          key: 'unit',
          value: '',
          options: this.unitVideoSelectList,
          style: {
            display: 'inline-flex'
          },
          onChange (index) {
            if (index !== vm.activeIndex) {
              vm.$refs.carousel.setActiveItem(index)
            }
          },
        },
      ],
      unitVideoValue: {}
    };
  },
  created () {
  },
  methods: {
    carouselChange (index, oldIndex) {
      this.$nextTick().then(() => {
        this.playerOptions.sources[0].src = this.companyVideoList[index].HDsrcP
      })
      this.activeIndex = index
      this.unitVideoValue = {
        unit: index
      }
      // this.playVideo(index, oldIndex)
    },
    playVideo (index, oldIndex) {
      // this.$nextTick().then(() => {
      //   const player = new window.EZUIKit.EZUIPlayer(`unit-video-${index}`);、
      //   player.play();
      // })
    }
  },
  computed: {
    companyVideoList () {
      const companyList = this.$store.state.sinuo.user.companyList
      const videoList = companyList.map(item => {
        return [
          ...item.video_info
        ]
      }).flat(1)
      var result = []
      videoList.forEach(element => {
        if (element.status) {
          result.push(element)
        }
      });
      return result
    },
    unitVideoSelectList () {
      return this.companyVideoList.map((item, index) => {
        return {
          label: item.unitName,
          value: index
        }
      })
    },
    activeVideoInfo () {
      return this.companyVideoList[this.activeIndex]
    },
    shouldPlayVideoUnit () {
      return this.$store.state.sinuo.dashboard.shouldPlayVideoUnit
    },
    cardTitle () {
      return (this.activeVideoInfo ? this.activeVideoInfo.unitName : '暂无视频')
    },
    needDecorateLineLength () {
      const title = this.cardTitle
      const titleLength = title.length
      const fullLineLength = 30
      const spaceLength = 4
      const needLength = fullLineLength - titleLength - spaceLength
      return needLength <= 20 ? needLength : 20
    }
  },
  watch: {
    unitVideoSelectList: {
      immediate: true,
      handler (val) {
        //  更新unit video list
        this.unitVideoConfig.find(item => {
          return item.key === 'unit'
        }).options = val
      }
    },
    shouldPlayVideoUnit (unit) {
      if (Object.keys(unit).length === 0) {
        return
      }
      const videoIndex = this.companyVideoList.findIndex(item => {
        return item.unitName === unit.video_info[0].unitName
      })
      this.$refs.carousel.setActiveItem(videoIndex)
    }
  }
}
</script>

<style scoped lang="scss">
.dash-card-wrapper{
  height: 410px;
  .content{
    padding-top: 10px;
    .unit-video{
      width: 100%;
      height: 100%;
      object-fit: contain;
    }
    .form-wrapper{
      margin-top: 10px;
    }
  }
}
</style>
