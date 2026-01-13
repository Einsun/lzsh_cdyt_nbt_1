<template>
  <div class="amap-wrapper">
    <div id="map-container"></div>
    <m3u8Video ref="ezuiVideo"></m3u8Video>
  </div>
</template>

<script>
import Vue from 'vue';
import m3u8Video from './components/video-point/Index'

export default {
  name: 'homepage-map',
  components: {
    m3u8Video,
  },
  mounted () {
    // this.createMap();
    // this.createMarker();
    // this.createMassMarks();
    // this.timer = setInterval(() => {
    //   this.index = this.companyList.length === this.index ? 1 : (this.index + 1)
    //   this.setMapCenter(this.index)
    // }, 10000)
  },
  beforeDestroy () {
    if (this.timer) {
      clearInterval(this.timer)
    }
  },
  data () {
    return {
      amapInstance: undefined,
      markerList: [],
      blinkIntervalId: 0,
      timer: {},
      index: 1,
      cluster: []
    };
  },
  computed: {
    companyList () {
      return this.$store.state.sinuo.user.companyList;
    },
    mapActiveUnit () {
      return this.$store.state.sinuo.dashboard.mapActiveUnit;
    }
  },
  watch: {
    companyList (newList, oldList) {
      this.removeMarker();
      // this.createMarker();
      this.createMassMarks();
      if (oldList.length === 0) {
        this.setMapCenter()
      }
    },
    mapActiveUnit (item) {
      this.moveToActiveUnit(item);
    },
  },
  methods: {
    createMap () {
      // layerList
      const layerList = [];
      //  building
      let buildingLayer = new AMap.Buildings({
        zIndex: 130,
        merge: false,
        sort: false,
        zooms: [17, 20],
        heightFactor: 2
      });
      layerList.push(buildingLayer);
      //  map
      const option = {
        mapStyle: 'amap://styles/blue',
        // showLabel: false,
        viewMode: '3D',
        features: ['bg', 'point', 'road'],
        pitch: 50,
        showIndoorMap: false,
        zoom: 14,
        resizeEnable: true,
        layers: [new AMap.TileLayer({}), ...layerList]
      };
      // center
      if (this.companyList.length > 0) {
        const firstCompany = this.companyList[0];
        const { lng, lat } = firstCompany;
        option.center = [lng, lat];
      }

      this.amapInstance = new AMap.Map('map-container', option);
      // this.amapInstance = new AMap.Map('map-container', {
      //   center: [113.680511, 34.784944],
      //   layers: [
      //     new AMap.TileLayer()
      //   ],
      //   zoom: 20
      // });
    },
    setMapCenter (index = 0) {
      if (this.companyList.length > 0) {
        const firstCompany = this.companyList[index] ? this.companyList[index] : this.companyList[0];
        const { lng, lat } = firstCompany;
        const center = [lng, lat];
        this.amapInstance.setCenter(center)
      }
    },
    createMassMarks () {
      if (this.amapInstance) {
        var markers = [];
        const alarmIcon = require('./images/icon/point_alarm.png')
        const faultIcon = require('./images/icon/point_fault.png')
        const normalIcon = require('./images/icon/point_normal.png')
        let points = this.companyList.filter(item => item.lng && item.lat).map(item => {
          const unitInfo = {
            lnglat: new AMap.LngLat(
              parseFloat(item.lng),
              parseFloat(item.lat)
            ),
            status: item.status,
            info: item
          }
          return unitInfo
        });
        // console.log('******* 123 ******', points)
        for (var i = 0; i < points.length; i += 1) {
          const options = {
            position: points[i]['lnglat'],
            offset: new AMap.Pixel(-15, -15),
            label: points[i]['info']['unitName'],
            status: points[i]['status']
          }
          const content = document.createElement('div')
          switch (points[i]['status']) {
            case 1: {
              content.innerHTML = `<img src="${alarmIcon}" class="alarm-marker blink-animate"/>`
              options.content = content
              options.zIndex = 300
              options.animation = 'AMAP_ANIMATION_DROP'
              options.extData = {
                needBlink: true
              }
              break
            }
            case 2: {
              options.icon = faultIcon
              options.zIndex = 200
              break
            }
            default: {
              options.icon = normalIcon
              options.zIndex = 100
              break
            }
          }
          var makera = new AMap.Marker(options);

          makera.on('click', (e) => {
            this.markerMassBindClick(e)
          })

          markers.push(makera)
        }

        var count = markers.length;
        var _renderClusterMarker = function (context) {
          var factor = Math.pow(context.count / count, 1 / 18);
          var div = document.createElement('div');
          var Hue = 180 - factor * 180;

          const markersList = context.markers
          var mapStatus = 0
          for (var index in markersList) {
            if (markersList[index].w.status === 1) {
              mapStatus = 1
            }
            if (markersList[index].w.status === 2) {
              mapStatus = 2
            }
          }
          var bgColor = 'hsla(217,50%,50%,1)';
          var borderColor = 'hsla(217,100%,40%,1)';
          var fontColor = 'hsla(' + Hue + ',100%,20%,1)';
          var shadowColor = 'hsla(' + Hue + ',100%,50%,1)';
          if (mapStatus === 1) {
            bgColor = 'hsla(0,50%,50%,1)';
            borderColor = 'hsla(0,100%,40%,1)';
            fontColor = 'hsla(' + Hue + ',100%,20%,1)';
            shadowColor = 'hsla(' + Hue + ',100%,50%,1)';
          } else if (mapStatus === 2) {
            bgColor = 'hsla(43,62%,50%,1)';
            borderColor = 'hsla(43,100%,40%,1)';
            fontColor = 'hsla(' + Hue + ',100%,20%,1)';
            shadowColor = 'hsla(' + Hue + ',100%,50%,1)';
          }

          div.style.backgroundColor = bgColor;
          var size = Math.round(30 + Math.pow(context.count / count, 1 / 5) * 20);
          div.style.width = div.style.height = size + 'px';
          div.style.border = 'solid 1px ' + borderColor;
          div.style.borderRadius = size / 2 + 'px';
          div.style.boxShadow = '0 0 1px ' + shadowColor;
          div.innerHTML = context.count;
          div.style.lineHeight = size + 'px';
          div.style.color = fontColor;
          div.style.fontSize = '14px';
          div.style.textAlign = 'center';
          context.marker.setOffset(new AMap.Pixel(-size / 2, -size / 2));
          context.marker.setContent(div)
        };
        this.addCluster(markers, _renderClusterMarker);
      }
    },
    markerMassBindClick (e) {
      const marker = e.target;
      const label = marker.getLabel();
      const unit = this.companyList.find(item => {
        return item.unitName === label;
      });
      if (unit) {
        this.openInfoWindow(unit);
      }
    },
    addCluster (markers, _renderClusterMarker) {
      if (this.cluster.length !== 0) {
        this.cluster.setMap(null);
      }
      this.cluster = new AMap.MarkerClusterer(this.amapInstance, markers, {
        gridSize: 60,
        renderClusterMarker: _renderClusterMarker,
        minClusterSize: 4
      });
    },
    createMarker () {
      if (this.amapInstance) {
        // const alarmIcon = require('./images/icon/alarm.png')
        // const faultIcon = require('./images/icon/fault.png')
        // const normalIcon = require('./images/icon/normal.png')
        const alarmIcon = require('./images/icon/point_alarm.png')
        const faultIcon = require('./images/icon/point_fault.png')
        const normalIcon = require('./images/icon/point_normal.png')
        const markerList = this.companyList.filter(item => item.lng && item.lat).map(item => {
          const options = {
            // 数据使用Number进行转换有可能出现NaN
            position: new AMap.LngLat(
              parseFloat(item.lng),
              parseFloat(item.lat)
            ),
            label: item.unitName,
          }
          const content = document.createElement('div')
          switch (item.status) {
            case 1: {
              content.innerHTML = `<img src="${alarmIcon}" class="alarm-marker blink-animate"/>`
              options.content = content
              options.zIndex = 300
              options.animation = 'AMAP_ANIMATION_DROP'
              options.extData = {
                needBlink: true
              }
              break
            }
            case 2: {
              options.icon = faultIcon
              options.zIndex = 200
              break
            }
            default: {
              options.icon = normalIcon
              options.zIndex = 100
              break
            }
          }
          return new AMap.Marker(options);
        });
        this.markerList = markerList;
        this.markerBindClick();
        this.amapInstance.add(markerList);
      }
    },
    removeMarker () {
      if (this.amapInstance) {
        this.amapInstance.remove(this.markerList);
      }
    },
    markerBindClick () {
      this.markerList.forEach(item => {
        item.on('click', this.markerClick);
      });
    },
    markerClick (e) {
      const marker = e.target;
      const label = marker.getLabel();
      // marker.setAnimation('AMAP_ANIMATION_BOUNCE')
      // setTimeout(() => {
      //   marker.setAnimation('AMAP_ANIMATION_NONE')
      // }, 1150)
      const unit = this.companyList.find(item => {
        return item.unitName === label;
      });
      if (unit) {
        this.openInfoWindow(unit);
      }
    },
    openInfoWindow (unit) {
      const vm = this;
      let content = `
        <div class="unit-window-wrapper">
          <div style="font-size: 14px;width: 350px;" class="info-window-wrapper">
            <p style="font-size: 16px;font-weight: bold;">${unit.unitName}</p>
            <p>单位类型：${unit.unitType}</p>
            <p>单位地址：${unit.address}</p>
            <p>负责人：${unit.managerName || '暂无'}</p>
            <p>联系电话：${unit.managerPhone || '暂无'}</p>
            <div class="button-wrapper">
              <button class="info-window-watch-alarm-btn el-button el-button--default ">查看报警</button>
              <button class="info-window-watch-video-btn el-button el-button--default">监控视频</button>
              <button class="info-window-watch-detail-btn el-button el-button--default">查看详情</button>
              <button class="info-window-reboot el-button el-button--default">复位</button>
            </div>
          </div>
        </div>
      `;

      const $infoWrapper = document.createElement('div');
      $infoWrapper.innerHTML = content;
      const infoWindow = new AMap.InfoWindow({
        anchor: 'bottom-centent',
        content: $infoWrapper,
        offset: new AMap.Pixel(0, -40)
      });
      infoWindow.open(this.amapInstance, [unit.lng, unit.lat]);
      this.$nextTick().then(() => {
        $infoWrapper.getElementsByClassName(
          'info-window-watch-alarm-btn'
        )[0].onclick = () => {
          vm.watchAlarm(unit);
        };
        $infoWrapper.getElementsByClassName(
          'info-window-watch-video-btn'
        )[0].onclick = () => {
          vm.watchVideo(unit);
        };
        $infoWrapper.getElementsByClassName(
          'info-window-watch-detail-btn'
        )[0].onclick = () => {
          vm.openDetailInfo(unit);
        };
        $infoWrapper.getElementsByClassName(
          'info-window-reboot'
        )[0].onclick = () => {
          vm.unitReboot(unit);
        };
      });
    },
    moveToActiveUnit (item) {
      this.amapInstance.setZoomAndCenter(17, [item.lng, item.lat]);
      this.openInfoWindow(item);
    },
    watchAlarm (unit) {
      //  to alarm filter
      this.$router.push({
        path: '/alarm/all',
        query: {
          unitID: unit.unitID
        }
      });
    },
    watchVideo (unit) {
      // if (unit.video_info.length !== 0) {
      //   this.$store.commit('sinuo/dashboard/setShouldPlayVideoUnit', {});
      //   this.$store.commit('sinuo/dashboard/setShouldPlayVideoUnit', unit);
      // } else {
      //   this.$message('该单位暂无视频');
      // }
      // console.log('****** unit ******', unit)
      this.$refs.ezuiVideo.open(unit.video_info[0].HDsrcP, unit.unitName)
    },
    openDetailInfo (unit) {
      // this.$cookieStore.delCookie('d2admin-1.7.0-token');
      // this.$cookieStore.delCookie('d2admin-1.7.0-uuid');
      // let md5String = 'sinuo_gangqu_2020';
      // let md5 = this.$MD5(md5String).toUpperCase();
      // let unitId = this.$Base64.encode(unit.unitID);
      // var baseUrl = 'http://82.156.19.203:81/' + '?&' + md5 + '&' + unitId + '&';
      // var p = baseUrl;
      // var a = document.createElement("a");
      // a.setAttribute("href", `${p}`);
      // a.setAttribute("target", "_blank");
      // a.click();
    },
    unitReboot (unit) {
      // console.log('****** 123 ******', unit)
      const id = unit.unitID
      const data = {
        id
      }
      this.removeMarker();
      this.$api.alarm.rebootAlarm({ data }).then(() => {
        // this.createMarker();
      })
    }
  }
};
</script>

<style scoped lang="scss">
.amap-wrapper {
  position: relative;
  width: 100%;
  height: 100vh;
  min-height: 1080px;
  #map-container {
    width: 100%;
    height: 100%;
    &::v-deep {
      .amap-info-sharp {
        border-left: 8px solid transparent;
        border-right: 8px solid transparent;
        border-top: 8px solid #fff;
        left: 50%;
        transform: translate(-50%, 0);
      }
      .amap-layer {
        filter: hue-rotate(20deg) brightness(0.6);
      }

      .unit-window-wrapper {
        display: flex;
        justify-content: space-between;
        .info-window-wrapper {
          height: 100%;
          .button-wrapper {
            margin-top: 20px;
            > button {
              width: 5em;
              height: 2em;
              padding: 0;
            }
          }
        }
        .video-window-wrapper {
          margin-right: 30px;
        }
      }

      .amap-marker-content{
        .blink-animate{
          animation: blink 2s infinite;
        }
        @keyframes blink {
          0%{
            opacity: 60%;
          }
          50%{
            opacity: 1;
          }
          100%{
            opacity: 60%;
          }
        }
      }
    }
  }
}
</style>
