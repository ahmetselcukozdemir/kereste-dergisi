var identID = 0;
CKEDITOR.dialog.add('uploadGalleryImages', function (editor) {
    return {
        title: 'Galeri İçin Resim Yükelemesi',
        resizable: CKEDITOR.DIALOG_RESIZE_BOTH,
        minWidth: 500,
        minHeight: 100,
        onOk: function (ev) {

            var iframe = (ev.sender.parts.contents).find('iframe.cke_dialog_ui_input_file');
            iframe = $(iframe['$'][0]).contents();
            var file = iframe.find('[name="galleryImgFiles"]')[0].files;
            if (!file || !file[0]) {
                alert("lütfen dosya seçiniz.");
                return false;
            }
            var formData = new FormData();


            for (let index = 0; index < file.length; index++) {
                formData.append('galleryFile[]', file[index]);
            }



            $.ajax({
                type: "POST",
                url: "/Ajax/UploadGallery",
                xhr: function () {
                    var myXhr = $.ajaxSettings.xhr();
                    if (myXhr.upload) {
                        myXhr.upload.addEventListener('progress', function (e) {

                        }, false);
                    }
                    return myXhr;
                },
                success: function (data) {
                    identID = (identID + 1) + $('#cke_editor1 iframe').length;
                    if (data.status == 200) {
                        var mySelection = editor.getSelection();

                        if (CKEDITOR.env.ie) {
                            mySelection.unlock(true);
                            selectedText = mySelection.getNative().createRange().text;
                        } else {
                            selectedText = mySelection.getNative();
                        }

                        editor.insertHtml(`<iframe class="swiper-gallery-iframe" id="swipeframe${identID}" style="width: 70%;height: 400px;border: none;"></iframe>`);

                        $('#cke_editor1 iframe').contents().find('#swipeframe' + identID).contents().find('head').append('<link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/Swiper/8.4.5/swiper-bundle.css">');


                        var st = $('#cke_editor1 iframe').contents().find('#swipeframe' + identID).contents().find('body');



                        var swiperItems = "";
                        data.urls.map(function (urlItem) {
                            swiperItems += `<div class="swiper-slide">
                                        <div class="content-gallery">
                                            <div class="img-wrapper">
                                                <img src="${urlItem}" width="100%">
                                            </div>
                                        </div>
                                    </div>`;
                        })

                        var response = '';
                        $.ajax({
                            type: "GET",
                            url: "https://cdnjs.cloudflare.com/ajax/libs/Swiper/8.4.5/swiper-bundle.min.js",
                            async: false,
                            success: function (text) {
                                st.append(` <div class="swiper content-gallery-slider">
                                        <div class="swiper-wrapper">
                                            ${swiperItems}
                                        </div>
                                        <div class="swiper-button-prev"></div>
                                        <div class="swiper-button-next"></div>
                                    </div>
                                    <scr`+ `ipt>
                                        ${text}
                                    </scr`+ `ipt>
                                    <scr`+ `ipt>
                                    const contentGalleryswiper = new Swiper('.content-gallery-slider', {
                                        speed: 400,
                                        spaceBetween: 100,
                                        navigation: {
                                            nextEl: '.swiper-button-next',
                                            prevEl: '.swiper-button-prev',
                                        },
                                    });
                                    </scr`+ `ipt>
                                    
                        `);

                                setTimeout(() => {
                                    st.find('script').remove();
                                }, 300);
                            }
                        });


                    }

                },
                error: function (error) {

                },
                async: true,
                data: formData,
                cache: false,
                contentType: false,
                processData: false,
                timeout: 60000
            });



        },
        contents: [
            {
                id: 'tab1',
                label: 'First Tab',
                title: 'First Tab Title',
                accessKey: 'Q',
                elements: [
                    {
                        type: 'file',
                        label: 'Resim Dosyalarını Seçiniz.',
                        id: 'galleryImgFiles',
                        onClick: function () {
                            var input = this.getInputElement();
                            input.$.accept = 'image/*';
                            input.$.multiple = true;
                        }
                    }
                ]
            }
        ]
    };
});

CKEDITOR.dialog.add('addtalklisten', function (editor) {
    return {
        title: 'Dinlenebilir Metin Yüklemesi',
        resizable: CKEDITOR.DIALOG_RESIZE_BOTH,
        minWidth: 500,
        minHeight: 100,
        onOk: function (ev) {

            var iframe = (ev.sender.parts.contents).find('iframe.cke_dialog_ui_input_file');
            iframe = $(iframe['$'][0]).contents();
            var file = iframe.find('[name="talkSoundFile"]')[0].files;
            if (!file || !file[0]) {
                alert("lütfen dosya seçiniz.");
                return false;
            }
            var formData = new FormData();
            formData.append('talkfile', file[0], file[0].name);
            $.ajax({
                type: "POST",
                url: "/Ajax/UploadSound",
                xhr: function () {
                    var myXhr = $.ajaxSettings.xhr();
                    if (myXhr.upload) {
                        myXhr.upload.addEventListener('progress', function (e) {

                        }, false);
                    }
                    return myXhr;
                },
                success: function (data) {

                    if (data.status == 200) {
                        var mySelection = editor.getSelection();

                        if (CKEDITOR.env.ie) {
                            mySelection.unlock(true);
                            selectedText = mySelection.getNative().createRange().text;
                        } else {
                            selectedText = mySelection.getNative();
                        }
                        editor.insertHtml('<strong class="speech-text notload-speech" data-speech="' + data.url + '"><i class="flaticon-381-microphone-1"></i>' + selectedText + "</strong>");



                        var mainElem = $('#cke_editor1 iframe').contents().find('.notload-speech');




                        mainElem.removeClass('notload-speech');
                        mainElem.prepend(`<span id="playArea"><svg xmlns="https://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="bi bi-play-fill" viewBox="0 0 16 16">
                          <path d="m11.596 8.697-6.363 3.692c-.54.313-1.233-.066-1.233-.697V4.308c0-.63.692-1.01 1.233-.696l6.363 3.692a.802.802 0 0 1 0 1.393z"/>
                        </svg></span>`);
                        mainElem.find('#playArea').off('click');
                        mainElem.find('#playArea').click(function () {
                            var elem = $(this).parent();

                            if (elem.find('audio').length <= 0) {
                                var talkedAudio = new Audio();
                                elem.append(talkedAudio);
                                talkedAudio.src = elem.data('speech');
                                elem.find('#playArea').find('svg').remove();

                                elem.find('#playArea').prepend(`<svg xmlns="https://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="bi bi-pause-fill" viewBox="0 0 16 16">
                                      <path d="M5.5 3.5A1.5 1.5 0 0 1 7 5v6a1.5 1.5 0 0 1-3 0V5a1.5 1.5 0 0 1 1.5-1.5zm5 0A1.5 1.5 0 0 1 12 5v6a1.5 1.5 0 0 1-3 0V5a1.5 1.5 0 0 1 1.5-1.5z"/>
                                    </svg>`)
                                talkedAudio.load();
                                talkedAudio.play();
                                talkedAudio.onended = function () {
                                    stopAllAudio();
                                }

                            } else {


                                if (elem.find('audio')[0].duration > 0 && elem.find('audio')[0].paused == false) {
                                    elem.find('#playArea').find('svg').remove();
                                    elem.find('#playArea').prepend(`<svg xmlns="https://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="bi bi-play-fill" viewBox="0 0 16 16">
                          <path d="m11.596 8.697-6.363 3.692c-.54.313-1.233-.066-1.233-.697V4.308c0-.63.692-1.01 1.233-.696l6.363 3.692a.802.802 0 0 1 0 1.393z"/>
                        </svg>`);
                                    elem.find('audio')[0].pause();
                                } else {
                                    stopAllAudio();
                                    elem.find('#playArea').find('svg').remove();
                                    elem.find('#playArea').prepend(`<svg xmlns="https://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="bi bi-pause-fill" viewBox="0 0 16 16">
                                      <path d="M5.5 3.5A1.5 1.5 0 0 1 7 5v6a1.5 1.5 0 0 1-3 0V5a1.5 1.5 0 0 1 1.5-1.5zm5 0A1.5 1.5 0 0 1 12 5v6a1.5 1.5 0 0 1-3 0V5a1.5 1.5 0 0 1 1.5-1.5z"/>
                                    </svg>`)
                                    elem.find('audio')[0].play();
                                }
                            }

                        })

                    }

                },
                error: function (error) {

                },
                async: true,
                data: formData,
                cache: false,
                contentType: false,
                processData: false,
                timeout: 60000
            });



        },
        contents: [
            {
                id: 'tab1',
                label: 'First Tab',
                title: 'First Tab Title',
                accessKey: 'Q',
                elements: [
                    {
                        type: 'file',
                        label: 'Ses Dosyası Seçiniz.',
                        id: 'talkSoundFile',
                        onClick: function () {
                            var input = this.getInputElement();
                            input.$.accept = 'audio/mp3';
                        }
                    }
                ]
            }
        ]
    };
});

CKEDITOR.plugins.add('insertGallery', {
    init: function (editor) {
        editor.addCommand('insertGalleryCmd', new CKEDITOR.dialogCommand('uploadGalleryImages'));
        editor.ui.addButton('insertGallery', {
            label: 'Galeri medyası yükle',
            command: 'insertGalleryCmd',
            toolbar: 'insert',
            icon: '/images/ckgallery.png'
        });

    }
});


CKEDITOR.plugins.add('insertSpech', {
    init: function (editor) {
        editor.addCommand('insertSpechCmd', new CKEDITOR.dialogCommand('addtalklisten'));
        editor.ui.addButton('insertSpech', {
            label: 'Konuşma medyası yükle',
            command: 'insertSpechCmd',
            toolbar: 'insert',
            icon: '/images/ckspeak.png'
        });

    }
});



var editor = CKEDITOR.replace('editor1', {
    extraPlugins: 'insertSpech,insertGallery,easyimage',
    removePlugins: 'image',
    cloudServices_uploadUrl: '/Admin/UploadImage?type=Files',
    cloudServices_tokenUrl: 'https://33333.cke-cs.com/token/dev/ijrDsqFix838Gh3wGO3F77FSW94BwcLXprJ4APSp3XQ26xsUHTi0jcb1hoBt',
});

editor.config.allowedContent = true;
editor.config.pasteFromWordRemoveFontStyles = false;
editor.config.pasteFromWordRemoveStyles = false;
editor.config.forcePasteAsPlainText = true;
editor.config.easyimage_class = null;



editor.on('instanceReady', function (evt) {
    $('#cke_editor1 iframe').contents().find('head').append(`<style>
                    #playArea{
                        cursor:pointer;
                               }                           
                    .speech-text{
                        background: rgb(248 255 85 / 27%);
                                 }
                       .speech-text svg{
                    position: relative;
                    bottom: -4px;
                                }
                </style>`);

    var mainElem = $('#cke_editor1 iframe').contents().find('.speech-text');


    mainElem.find('#playArea').off('click');
    mainElem.find('#playArea').click(function () {
        var elem = $(this).parent();

        if (elem.find('audio').length <= 0) {
            var talkedAudio = new Audio();
            elem.append(talkedAudio);
            talkedAudio.src = elem.data('speech');
            elem.find('#playArea').find('svg').remove();

            elem.find('#playArea').prepend(`<svg xmlns="https://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="bi bi-pause-fill" viewBox="0 0 16 16">
                                <path d="M5.5 3.5A1.5 1.5 0 0 1 7 5v6a1.5 1.5 0 0 1-3 0V5a1.5 1.5 0 0 1 1.5-1.5zm5 0A1.5 1.5 0 0 1 12 5v6a1.5 1.5 0 0 1-3 0V5a1.5 1.5 0 0 1 1.5-1.5z"/>
                            </svg>`)
            talkedAudio.load();
            talkedAudio.play();
            talkedAudio.onended = function () {
                stopAllAudio();
            }

        } else {


            if (elem.find('audio')[0].duration > 0 && elem.find('audio')[0].paused == false) {
                elem.find('#playArea').find('svg').remove();
                elem.find('#playArea').prepend(`<svg xmlns="https://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="bi bi-play-fill" viewBox="0 0 16 16">
                    <path d="m11.596 8.697-6.363 3.692c-.54.313-1.233-.066-1.233-.697V4.308c0-.63.692-1.01 1.233-.696l6.363 3.692a.802.802 0 0 1 0 1.393z"/>
                </svg>`);
                elem.find('audio')[0].pause();
            } else {
                stopAllAudio();
                elem.find('#playArea').find('svg').remove();
                elem.find('#playArea').prepend(`<svg xmlns="https://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="bi bi-pause-fill" viewBox="0 0 16 16">
                                <path d="M5.5 3.5A1.5 1.5 0 0 1 7 5v6a1.5 1.5 0 0 1-3 0V5a1.5 1.5 0 0 1 1.5-1.5zm5 0A1.5 1.5 0 0 1 12 5v6a1.5 1.5 0 0 1-3 0V5a1.5 1.5 0 0 1 1.5-1.5z"/>
                            </svg>`)
                elem.find('audio')[0].play();
            }
        }

    })








    $('#cke_editor1 iframe').contents().find('.content-gallery-slider').each(function (index, elem) {

        identID = (identID + 1);
        var swiperRunner = `<iframe class="swiper-gallery-iframe" id="swipeframe${identID}" style="width: 70%;height: 400px;border: none;"></iframe>`;
        $(elem).after(swiperRunner);
        $('#cke_editor1 iframe').contents().find('#swipeframe' + identID).contents().find('head').append('<link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/Swiper/8.4.5/swiper-bundle.css">');
        var st = $('#cke_editor1 iframe').contents().find('#swipeframe' + identID).contents().find('body');

        $.ajax({
            type: "GET",
            url: "https://cdnjs.cloudflare.com/ajax/libs/Swiper/8.4.5/swiper-bundle.min.js",
            async: false,
            success: function (text) {
                st.append(` 
                        <div class="swiper content-gallery-slider">${$(elem).html()}</div>
                        <scr`+ `ipt>
                            ${text}
                        </scr`+ `ipt>
                        <scr`+ `ipt>
                        const contentGalleryswiper = new Swiper('.content-gallery-slider', {
                            speed: 400,
                            spaceBetween: 100,
                            navigation: {
                                nextEl: '.swiper-button-next',
                                prevEl: '.swiper-button-prev',
                            },
                        });
                        </scr`+ `ipt>
                        
            `);

                // setTimeout(() => {
                //     st.find('script').remove();
                // }, 300);
            }
        });

        $(elem).remove();
    });


});



$('form:has(#editor1)').on('submit', function (e) {

    $('#cke_editor1 iframe').contents().find('img').each((index, elem) => {
        $(elem).addClass('content-img').addClass('lazyload').attr('width', elem.naturalWidth).attr('height', elem.naturalHeight).attr('data-src', elem.src).removeAttr('src');
    })


    $('#cke_editor1 iframe').contents().find('.swiper-gallery-iframe').each(function (index, elem) {
        $(elem).contents().find('.content-gallery-slider').each(function (idx, elm) {
            var swiperItems = "";
            $(elm).find('.swiper-slide img').each(function (ix, el) {
                swiperItems += `<div class="swiper-slide">
                                    <img src="${el.src}" width="100%">
                                </div>`;
            });

            var swiperHtmlData = `
                <div class="swiper content-gallery-slider">
                    <div class="swiper-wrapper">
                        ${swiperItems}
                    </div>
                    <div class="swiper-button-prev"></div>
                    <div class="swiper-button-next"></div>
                </div>
            `;

            $(elem).after(swiperHtmlData);
            $(elem).remove();

        })


    });

    stopAllAudio();

})

function stopAllAudio() {
    $('#cke_editor1 iframe').contents().find('.speech-text').each((idx, elem) => {
        $(elem).find('#playArea').find('svg').remove();
        $(elem).find('#playArea').prepend(`<svg xmlns="https://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="bi bi-play-fill" viewBox="0 0 16 16">
                                                                        <path d="m11.596 8.697-6.363 3.692c-.54.313-1.233-.066-1.233-.697V4.308c0-.63.692-1.01 1.233-.696l6.363 3.692a.802.802 0 0 1 0 1.393z"/>
                                                                    </svg>`);
        $(elem).find('audio')[0].pause();
    })
}
