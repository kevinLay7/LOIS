var webpack = require('webpack');
var path = require('path');
const BundleAnalyzerPlugin = require('webpack-bundle-analyzer').BundleAnalyzerPlugin;
const UglifyJsPlugin = require('uglifyjs-webpack-plugin');

var apiHost;
var setupApi = function(){
  switch(process.env.NODE_ENV){
      case 'production':
        apiHost = "'http://loispc.apollo-lab.com/api'";
        break;
      default:
        apiHost = "'http://localhost:55547/api'";
        break;
  }
};
setupApi();


module.exports = {
    entry: { 'main': './ClientApp/src/main.js' },
    output: {
    path: path.resolve(__dirname, './Scripts/dist/'),
    publicPath: 'Scripts/dist/',
    filename: 'bundle.js'
    },
    resolve: {
    extensions: ['.js', '.vue', '.json', '.css'],
    alias: {
      'vue$': 'vue/dist/vue.esm.js'
    }
    },
    module: {
    loaders: [
      {
        test: /\.vue$/,
        loader: 'vue-loader',
        options: {
          loaders: {
          }
        }
      },
        {
        test: /\.js$/,
        loader: 'babel-loader',
        exclude: /node_modules/
        },
      {
        test: /\.(scss)$/,
          use: [{
              loader: 'style-loader', // inject CSS to page
          }, {
              loader: 'css-loader', // translates CSS into CommonJS modules
          }, {
              loader: 'postcss-loader', // Run post css actions
              options: {
                  plugins: function () { // post css plugins, can be exported to postcss.config.js
                      return [
                          require('precss'),
                          require('autoprefixer')
                      ];
                  }
              }
          }, {
              loader: 'sass-loader' // compiles Sass to CSS
          }]
      },
      {
        test: /\.css$/,
        use: ['style-loader', 'css-loader']
      },
      {
        test: /\.(eot|ttf|woff|woff2)(\?\S*)?$/,
        loader: 'file-loader'
      },
      {
        test: /\.(png|jpe?g|gif)(\?\S*)?$/,
        loader: 'file-loader',
        query: {
          name: '[name].[ext]?[hash]'
        }

      },
      { test: /\.svg/, loader: 'file-loader' }
    ]
    },
    devtool: "source-map",
    devServer: {
      proxy: {
        '*': {
          target: 'http://localhost:55547',
          changeOrigin: true
        }
      }
    },
    plugins: [
        new webpack.DefinePlugin({
            __API__: apiHost
        })
    ]
};

if (process.env.NODE_ENV === 'production') {
  module.exports.devtool = 'source-map';
  // http://vue-loader.vuejs.org/en/workflow/production.html
  module.exports.plugins = (module.exports.plugins || []).concat([
    new webpack.DefinePlugin({
      $: 'jquery',
      jquery: 'jquery',
      'window.jQuery': 'jquery',
      jQuery: 'jquery'
    }),
    new webpack.DefinePlugin({
      'process.env': {
        NODE_ENV: '"production"'
      }
    }),
      new UglifyJsPlugin({
          uglifyOptions: {
              ecma: 8
          }
      }),
      new webpack.LoaderOptionsPlugin({
          minimize: true
      }),
      new webpack.IgnorePlugin(/^\.\/locale$/, /moment$/)
  ])
}
